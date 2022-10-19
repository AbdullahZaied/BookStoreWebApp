using Data.Access.Layer.Data;
using Data.Access.Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Data.Access.Layer.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }

        private string GenerateAccessToken(ApplicationUser user, List<Claim> authClaims)
        {
            _ = int.TryParse(configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"]));

            var newAccessToken = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(newAccessToken);
        }
        public async Task<string> RefreshToken(string? currentAccessToken)
        {
            var principal = GetPrincipalFromExpiredToken(currentAccessToken);
            if (principal == null)
            {
                return null;
            }

            string? username = principal.Identity?.Name;

            var user = await userManager.FindByNameAsync(username);
            string? refreshToken = user.RefreshToken;

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return null;
            }

            var newAccessToken = GenerateAccessToken(user, principal.Claims.ToList());

            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await userManager.UpdateAsync(user);

            return newAccessToken;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModelData signUpModel)
        {
            var userExists = await userManager.FindByNameAsync(signUpModel.UserName);

            if (userExists == null)
            {
                var user = new ApplicationUser()
                {
                    FirstName = signUpModel.FirstName,
                    LastName = signUpModel.LastName,
                    Email = signUpModel.Email,
                    UserName = signUpModel.UserName,
                };

                return await userManager.CreateAsync(user, signUpModel.Password);
            }
            else
            {
                return null;
            }

        }

        public async Task<string> LoginAsync(SignInModelData signInModel)
        {
            var signIn = await signInManager.PasswordSignInAsync(signInModel.UserName, signInModel.Password, true, false);
            if (!signIn.Succeeded)
            {
                return null;
            }

            var user = await userManager.FindByNameAsync(signInModel.UserName);
            var roles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, signInModel.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
                var roleInDb = await roleManager.FindByNameAsync(role);
                if (roleInDb != null)
                {
                    var roleClaims = await roleManager.GetClaimsAsync(roleInDb);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        authClaims.Add(roleClaim);
                    }
                }
            }


            var jwtToken = GenerateAccessToken(user, authClaims);

            _ = int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out int RefreshTokenValidityInDays);
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(RefreshTokenValidityInDays);
            await userManager.UpdateAsync(user);

            return jwtToken;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}

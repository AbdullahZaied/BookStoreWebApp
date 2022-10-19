using Data.Access.Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Data.Access.Layer.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignUpModelData signUpModel);
        Task<string> LoginAsync(SignInModelData signInModel);
        Task<string> RefreshToken(string? currentAccessToken);
        Task LogoutAsync();
    }
}

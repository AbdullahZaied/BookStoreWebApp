using Business.Logic.Layer.Models;

namespace Business.Logic.Layer.Services
{
    public interface IAccountService
    {
        Task<string> logIn(SignInModelBusiness signInModel);
        Task<bool> signUp(SignUpModelBusiness signUpModel);
        string? GetCurrentUserId();
        Task<string> RefreshToken();
        Task LogoutAsync();
    }
}

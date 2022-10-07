using AutoMapper;
using Business.Logic.Layer.Models;
using Data.Access.Layer.Models;
using Data.Access.Layer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Business.Logic.Layer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _accountRepository = accountRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public string? GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }

        public async Task<string> RefreshToken()
        {
            string? currentAccessToken = _httpContextAccessor.HttpContext?.Request.Headers["authorization"];
            currentAccessToken = currentAccessToken?.Replace("Bearer ", "");
            return await _accountRepository.RefreshToken(currentAccessToken);
        }

        public async Task<bool> signUp(SignUpModelBusiness signUpModel)
        {
            var signupResult = await _accountRepository.SignUpAsync(_mapper.Map<SignUpModelData>(signUpModel));
            return signupResult.Succeeded;
        }

        public async Task<string> logIn(SignInModelBusiness signInModel)
        {
            var signinResult = await _accountRepository.LoginAsync(_mapper.Map<SignInModelData>(signInModel));
            return signinResult;
        }
    }
}

using AutoMapper;
using BookStoreAPI.Models;
using Business.Logic.Layer.Models;
using Business.Logic.Layer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var newAccessToken = await _accountService.RefreshToken();
            if(newAccessToken == null)
            {
                return BadRequest();
            }
            else
            { 
                return Ok(newAccessToken);
            }
        }
        [HttpPost("signup")]
        public async Task<IActionResult> signUp([FromBody] SignUpModelApi signUpModel)
        {
            var signupSucceed = await _accountService.signUp(_mapper.Map<SignUpModelBusiness>(signUpModel));

            if (signupSucceed)
            {
                return Ok(signupSucceed);
            }

            return Unauthorized();
        }

        [HttpPost("login")]
        public async Task<IActionResult> logIn([FromBody] SignInModelApi signInModel)
        {
            var signinResult = await _accountService.logIn(_mapper.Map<SignInModelBusiness>(signInModel));

            if (string.IsNullOrEmpty(signinResult))
            {
                return Unauthorized();
            }

            return Ok(signinResult);
        }

        [HttpGet("logout")]
        public async Task LogoutAsync()
        {
            try
            {
                await _accountService.LogoutAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

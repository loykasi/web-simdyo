using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Abstracts;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;

namespace Scratch.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController(IAccountService accountService) : BaseController
    {
        private readonly IAccountService _accountService = accountService;

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var result = await _accountService.RegisterAsync(registerRequest);

            return ToApiResult(result);
        }

        [HttpPost]
        [Route("confirm-email")]
        public async Task<IActionResult> Confirm(ConfirmEmailRequest request)
        {
            var result = await _accountService.ConfirmEmail(request);

            return ToApiResult(result);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var result = await _accountService.LoginAsync(loginRequest);

            return ToApiResult(result);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = HttpContext.Request.Cookies["REFRESH_TOKEN"];

            await _accountService.RefreshTokenAsync(refreshToken);

            return Ok();
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPassworedRquest)
        {
            await _accountService.ForgotPassword(forgotPassworedRquest);

            return Ok();
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            await _accountService.ResetPassword(resetPasswordRequest);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _accountService.GetProfile();

            return ToApiResult(result);
        }
    }
}

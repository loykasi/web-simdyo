using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController(IAccountService accountService) : BaseController
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var result = await accountService.RegisterAsync(registerRequest);

            return ToApiResult(result);
        }

        [HttpPost]
        [Route("confirm-email")]
        public async Task<IActionResult> Confirm(ConfirmEmailRequest request)
        {
            var result = await accountService.ConfirmEmail(request);

            return ToApiResult(result);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var result = await accountService.LoginAsync(loginRequest);

            return ToApiResult(result);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = HttpContext.Request.Cookies["REFRESH_TOKEN"];

            var result = await accountService.RefreshTokenAsync(refreshToken);

            return ToApiResult(result);
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPassworedRquest)
        {
            var result = await accountService.ForgotPassword(forgotPassworedRquest);

            return ToApiResult(result);
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var result = await accountService.ResetPassword(resetPasswordRequest);

            return ToApiResult(result);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await accountService.LogOut();

            return ToApiResult(result);
        }

        [HttpGet]
        [Authorize]
        [Route("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var result = await accountService.GetProfile();

            return ToApiResult(result);
        }
    }
}

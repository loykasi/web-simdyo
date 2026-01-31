using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Interfaces.Services;
using Scratch.Application.Models.Requests;

namespace Scratch.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController(IAuthService accountService, IEmailService emailService) : BaseController
    {
        [HttpPost]
        [Route("send-otp")]
        public async Task<IActionResult> RequestLogin([FromQuery] string email)
        {
            var result = await accountService.RequestLoginAsync(email);

            return ToApiResult(result);
        }

        [HttpPost]
        [Route("login-otp")]
        public async Task<IActionResult> LoginWithOtp(LoginOtpRequest request)
        {
            var result = await accountService.LoginWithOtpAsync(request);

            return ToApiResult(result);
        }

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
        [Route("user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await accountService.GetCurrentUser();

            return ToApiResult(result);
        }
    }
}

using Application.Interfaces.Services;
using Application.Models.Requests.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : BaseController
    {
        [HttpPost]
        [Route("send-otp")]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> RequestLogin([FromQuery] string email)
        {
            var result = await authService.RequestLoginAsync(email);
            return ToApiResult(result);
        }

        [HttpPost]
        [Route("login-otp")]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> LoginWithOtp(LoginOtpRequest request)
        {
            var result = await authService.LoginWithOtpAsync(request);
            return ToApiResult(result);
        }

        [HttpPost]
        [Route("register")]
        [EnableRateLimiting("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var result = await authService.RegisterAsync(registerRequest);
            return ToApiResult(result);
        }

        [HttpPost]
        [Route("confirm-email")]
        public async Task<IActionResult> Confirm(ConfirmEmailRequest request)
        {
            var result = await authService.ConfirmEmail(request);
            return ToApiResult(result);
        }

        [HttpPost]
        [Route("login")]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var result = await authService.LoginAsync(loginRequest);
            return ToApiResult(result);
        }

        [HttpPost]
        [Authorize]
        [Route("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = HttpContext.Request.Cookies["REFRESH_TOKEN"];
            var result = await authService.RefreshTokenAsync(refreshToken);
            return ToApiResult(result);
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPassworedRquest)
        {
            var result = await authService.ForgotPassword(forgotPassworedRquest);
            return ToApiResult(result);
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var result = await authService.ResetPassword(resetPasswordRequest);
            return ToApiResult(result);
        }

        [HttpPost]
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await authService.LogOut();
            return ToApiResult(result);
        }

        [HttpGet]
        [Authorize]
        [Route("user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await authService.GetCurrentUser();
            return ToApiResult(result);
        }
    }
}

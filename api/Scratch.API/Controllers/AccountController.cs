using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Interfaces.Services;
using Scratch.Application.Models.Requests;

namespace Scratch.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController(IAuthService authService) : BaseController
    {
        [HttpPost]
        [Authorize]
        [Route("change-password")]
        public async Task<IActionResult> ResetPassword(ChangePasswordRequest changePasswordRequest)
        {
            var result = await authService.ChangePassword(changePasswordRequest);

            return ToApiResult(result);
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            var result = await authService.GetProfileDetail(username);

            return ToApiResult(result);
        }
    }
}

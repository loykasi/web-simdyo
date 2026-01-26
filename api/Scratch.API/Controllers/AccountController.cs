using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Requests;

namespace Scratch.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController(IAccountService accountService, ICurrentUserService currentUserService) : BaseController
    {
        [HttpPost]
        [Authorize]
        [Route("change-password")]
        public async Task<IActionResult> ResetPassword(ChangePasswordRequest changePasswordRequest)
        {
            var result = await accountService.ChangePassword(changePasswordRequest);

            return ToApiResult(result);
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            var result = await accountService.GetProfileDetail(username);

            return ToApiResult(result);
        }
    }
}

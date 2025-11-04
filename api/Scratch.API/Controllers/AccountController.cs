using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Abstracts;
using Scratch.Domain.Requests;

namespace Scratch.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController(IAccountService accountService) : BaseController
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
        [Route("{userName}")]
        public async Task<IActionResult> GetProfile(string userName)
        {
            var result = await accountService.GetProfileDetail(userName);

            return ToApiResult(result);
        }
    }
}

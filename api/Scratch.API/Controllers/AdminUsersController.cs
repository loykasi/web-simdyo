using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Interfaces.Services;
using Scratch.Domain.Requests;

namespace Scratch.API.Controllers
{
    [Route("api/admin/users")]
    public class AdminUsersController
    (
        IUserBanService userBanService
    ) : BaseController
    {
        [HttpPost("{userId}/ban")]
        public async Task<IActionResult> Ban(Guid userId, UserBanRequest payload)
        {
            var result = await userBanService.Ban(userId, payload);

            return ToApiResult(result);
        }

        [HttpDelete("{userId}/ban")]
        public async Task<IActionResult> Unban(Guid userId)
        {
            var result = await userBanService.RevokeBan(userId);

            return ToApiResult(result);
        }
    }
}

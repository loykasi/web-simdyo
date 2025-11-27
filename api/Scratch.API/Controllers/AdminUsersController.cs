using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Authorization;
using Scratch.Application.Interfaces.Services;
using Scratch.Application.Services;
using Scratch.Domain.Authorizations;
using Scratch.Domain.Requests;

namespace Scratch.API.Controllers
{
    [Route("api/admin/users")]
    [RequirePermission(Permissions.DashboardAccess)]
    [RequirePermission(Permissions.ManageUsers)]
    public class AdminUsersController
    (
        IUserService userService,
        IUserBanService userBanService
    ) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] string? searchTerm, [FromQuery] int? pageNumber, [FromQuery] int? limit)
        {
            var result = await userService.Get(searchTerm, pageNumber, limit);

            return ToApiResult(result);
        }

        [HttpPut("{id}/roles")]
        public async Task<IActionResult> SetRole(string id, SetUserRoleRequest payload)
        {
            var result = await userService.SetRole(id, payload);

            return ToApiResult(result);
        }

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

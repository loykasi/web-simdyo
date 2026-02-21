using Application.Authorization;
using Application.Interfaces.Services;
using Application.Models.Requests.Account;
using Application.Models.Requests.Role;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
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
        public async Task<IActionResult> Ban(int userId, UserBanRequest payload)
        {
            var result = await userBanService.Ban(userId, payload);

            return ToApiResult(result);
        }

        [HttpDelete("{userId}/ban")]
        public async Task<IActionResult> Unban(int userId)
        {
            var result = await userBanService.RevokeBan(userId);

            return ToApiResult(result);
        }
    }
}

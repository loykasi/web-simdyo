using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Authorization;
using Scratch.Application.Interfaces.Services;
using Scratch.Application.Services;
using Scratch.Domain.Authorizations;
using Scratch.Domain.Requests;

namespace Scratch.API.Controllers
{
    [Route("api/admin/projects")]
    [RequirePermission(Permissions.DashboardAccess)]
    [RequirePermission(Permissions.ManageProjects)]
    public class AdminProjectsController
    (
        IProjectBanService projectBanService,
        IProjectService projectService
    ) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string? search,
            [FromQuery] int? page,
            [FromQuery] int? limit
        )
        {
            var result = await projectService.GetAll(search, page, limit);

            return ToApiResult(result);
        }

        [HttpPost("{publicId}/ban")]
        [Authorize]
        public async Task<IActionResult> Ban(string publicId, BanProjectRequest payload)
        {
            var result = await projectBanService.AddBan(publicId, payload);
            
            return ToApiResult(result);
        }

        [HttpDelete("{publicId}/ban")]
        [Authorize]
        public async Task<IActionResult> Unban(string publicId)
        {
            var result = await projectBanService.RevokeBan(publicId);

            return ToApiResult(result);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Authorization;
using Scratch.Application.Interfaces.Services;
using Scratch.Application.Models.Requests;

namespace Scratch.API.Controllers
{
    [Route("api/projects")]
    public class ProjectReportsController(IProjectReportService projectReportService) : BaseController
    {
        [HttpGet("reports")]
        [RequirePermission(Permissions.DashboardAccess)]
        [RequirePermission(Permissions.ManageProjectReport)]
        public async Task<IActionResult> Get(
            [FromQuery] string? filter, [FromQuery] int? page, [FromQuery] int? limit)
        {
            var result = await projectReportService.Get(filter, page, limit);

            return ToApiResult(result);
        }

        [HttpPost("{publicId}/reports")]
        [Authorize]
        public async Task<IActionResult> Report(string publicId, ReportProjectRequest payload)
        {
            var result = await projectReportService.AddReport(publicId, payload);

            return ToApiResult(result);
        }
    }
}

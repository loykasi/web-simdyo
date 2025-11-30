using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Scratch.Application.Authorization;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Authorizations;
using Scratch.Domain.Requests;

namespace Scratch.API.Controllers
{
    [Route("api/projects")]
    public class ProjectController
    (
        IProjectService projectService
    ): BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetProjects(
            [FromQuery] string? q,
            [FromQuery] string? category,
            [FromQuery] int? lastId,
            [FromQuery] int? limit)
        {
            var result = await projectService.GetProjectsAsync(q, category, lastId, limit);

            return ToApiResult(result);
        }

        [HttpGet("users/{userName}")]
        public async Task<IActionResult> GetUsersProjects(string userName, [FromQuery] int? page, [FromQuery] int? limit)
        {
            var result = await projectService.GetUserProjects(userName, page, limit);

            return ToApiResult(result);
        }

        [HttpGet("users/trash")]
        [Authorize]
        public async Task<IActionResult> GetUsersTrash([FromQuery] int? page, [FromQuery] int? limit)
        {
            var result = await projectService.GetUserTrashAsync(page, limit);

            return ToApiResult(result);
        }

        [HttpGet("{publicId}")]
        public async Task<IActionResult> GetProject(string publicId)
        {
            var result = await projectService.Get(publicId);

            return ToApiResult(result);
        }

        [HttpPost]
        [Authorize]
        [Route("upload")]
        public async Task<IActionResult> UploadProject(UploadProjectRequest uploadProjectRequest)
        {
            var result = await projectService.Upload(uploadProjectRequest);

            return ToApiResult(result);
        }

        [HttpPut]
        [Authorize]
        [Route("{publicId}")]
        public async Task<IActionResult> UpdateProject(string publicId, UpdateProjectRequest updateProjectRequest)
        {
            var result = await projectService.Update(publicId, updateProjectRequest);

            return ToApiResult(result);
        }

        [HttpDelete]
        [Authorize]
        [Route("{publicId}")]
        public async Task<IActionResult> SoftDelete(string publicId)
        {
            var result = await projectService.Delete(publicId);

            return ToApiResult(result);
        }

        [HttpPost]
        [Authorize]
        [Route("{publicId}/restore")]
        public async Task<IActionResult> Undelete(string publicId)
        {
            var result = await projectService.Undelete(publicId);

            return ToApiResult(result);
        }
    }
}

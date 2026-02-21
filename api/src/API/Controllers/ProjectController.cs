using Application.Interfaces.Services;
using Application.Models.Requests;
using Application.Models.Requests.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/projects")]
    public class ProjectController
    (
        IProjectService projectService
    ): BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetProjects([FromQuery] GetProjectsQuery query)
        {
            var result = await projectService.GetProjectsAsync(query);
            return ToApiResult(result);
        }

        [HttpGet("users/{userName}")]
        public async Task<IActionResult> GetUsersProjects(string userName, [FromQuery] PaginationQuery parameter)
        {
            var result = await projectService.GetUserProjects(userName, parameter);
            return ToApiResult(result);
        }

        [HttpGet("users/trash")]
        [Authorize]
        public async Task<IActionResult> GetUsersTrash([FromQuery] PaginationQuery parameter)
        {
            var result = await projectService.GetUserTrashAsync(parameter);
            return ToApiResult(result);
        }

        [HttpGet("{publicId}")]
        public async Task<IActionResult> GetProject(string publicId)
        {
            var result = await projectService.Get(publicId);
            return ToApiResult(result);
        }

        [HttpGet("upload-limit")]
        [Authorize]
        public async Task<IActionResult> GetDailyLimit()
        {
            var result = await projectService.GetDailyLimit();
            return ToApiResult(result);
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> RequestUpload(UploadProjectRequest uploadProjectRequest)
        {
            var result = await projectService.RequestUpload(uploadProjectRequest);
            return ToApiResult(result);
        }

        [HttpPut("{publicId}")]
        [Authorize]
        public async Task<IActionResult> UpdateProject(string publicId, UpdateProjectRequest updateProjectRequest)
        {
            var result = await projectService.Update(publicId, updateProjectRequest);
            return ToApiResult(result);
        }

        [HttpDelete("{publicId}")]
        [Authorize]
        public async Task<IActionResult> SoftDelete(string publicId)
        {
            var result = await projectService.Delete(publicId);
            return ToApiResult(result);
        }

        [HttpPost("{publicId}/restore")]
        [Authorize]
        public async Task<IActionResult> Undelete(string publicId)
        {
            var result = await projectService.Undelete(publicId);
            return ToApiResult(result);
        }
    }
}

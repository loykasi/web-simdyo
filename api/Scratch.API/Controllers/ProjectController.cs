using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Abstracts;
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
        public async Task<IActionResult> GetProjects()
        {
            var result = await projectService.GetProjectsAsync();

            return ToApiResult(result);
        }

        [HttpGet("user/{userName}")]
        public async Task<IActionResult> GetUsersProject(string userName)
        {
            var result = await projectService.GetUserProjects(userName);

            return ToApiResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(string id)
        {
            var result = await projectService.Get(id);

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
    }
}

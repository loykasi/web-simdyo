using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Interfaces.Services;
using Scratch.Application.Models.Requests;

namespace Scratch.API.Controllers
{
    [Route("api/projects")]
    public class ProjectCommentsController(IProjectCommentService projectCommentService) : BaseController
    {
        [HttpGet("{publicId}/comments")]
        public async Task<IActionResult> GetComments(string publicId, [FromQuery] int? limit, [FromQuery] int? lastId, [FromQuery] int? parentId)
        {
            var result = await projectCommentService.GetComments(publicId, limit, lastId, parentId);

            return ToApiResult(result);
        }

        [HttpPost("{publicId}/comments")]
        [Authorize]
        public async Task<IActionResult> Add(string publicId, AddCommentRequest addCommentRequest)
        {
            var result = await projectCommentService.Add(publicId, addCommentRequest);

            return ToApiResult(result);
        }

        [HttpDelete("{publicId}/comments")]
        [Authorize]
        public async Task<IActionResult> Add(string publicId, [FromQuery] int commentId)
        {
            var result = await projectCommentService.Remove(publicId, commentId);

            return ToApiResult(result);
        }
    }
}
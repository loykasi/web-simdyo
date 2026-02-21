using Application.Interfaces.Services;
using Application.Models.Requests.ProjectReaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/projects")]
    public class ProjectReactionsController(IProjectReactionService projectLikeService) : BaseController
    {
        [HttpGet("{publicId}/reaction-status")]
        [Authorize]
        public async Task<IActionResult> GetStatus(string publicId)
        {
            var result = await projectLikeService.GetReactionStatus(publicId);
            return ToApiResult(result);
        }

        [HttpPost("{publicId}/reaction")]
        [Authorize]
        public async Task<IActionResult> Add(string publicId, AddReactionRequest request)
        {
            var result = await projectLikeService.AddReaction(publicId, request);
            return ToApiResult(result);
        }

        [HttpDelete("{publicId}/reaction")]
        [Authorize]
        public async Task<IActionResult> Delete(string publicId)
        {
            var result = await projectLikeService.DeleteReaction(publicId);
            return ToApiResult(result);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Abstracts;

namespace Scratch.API.Controllers
{
    [Route("api/projects")]
    public class ProjectLikesController(IProjectLikeService projectLikeService) : BaseController
    {
        [HttpGet("{publicId}/like")]
        public async Task<IActionResult> GetLikeCount(string publicId)
        {
            var result = await projectLikeService.GetLike(publicId);

            return ToApiResult(result);
        }

        [HttpGet("{publicId}/like-status")]
        [Authorize]
        public async Task<IActionResult> GetLikeStatus(string publicId)
        {
            var result = await projectLikeService.GetLikeStatus(publicId);

            return ToApiResult(result);
        }

        [HttpPost("{publicId}/like")]
        [Authorize]
        public async Task<IActionResult> Like(string publicId)
        {
            var result = await projectLikeService.AddLike(publicId);

            return ToApiResult(result);
        }

        [HttpDelete("{publicId}/like")]
        [Authorize]
        public async Task<IActionResult> DeleteLike(string publicId)
        {
            var result = await projectLikeService.DeleteLike(publicId);

            return ToApiResult(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Interfaces.Services;

namespace Scratch.API.Controllers
{
    [Route("api/users")]
    public class UsersController
    (
        IUserService userService
    ): BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetProjects([FromQuery] int? pageNumber, [FromQuery] int? limit)
        {
            var result = await userService.Get(pageNumber, limit);

            return ToApiResult(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Interfaces.Services;
using Scratch.Domain.Requests;

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

        [HttpPut("{id}/roles")]
        public async Task<IActionResult> SetRole(string id, SetUserRoleRequest payload)
        {
            var result = await userService.SetRole(id, payload);

            return ToApiResult(result);
        }
    }
}

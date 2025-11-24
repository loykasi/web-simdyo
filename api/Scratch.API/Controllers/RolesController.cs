using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Interfaces.Services;
using Scratch.Domain.Authorizations;
using Scratch.Domain.Requests;

namespace Scratch.API.Controllers
{
    [Route("api/roles")]
    public class RolesController(IRoleService roleService) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await roleService.GetAll();

            return ToApiResult(result);
        }

        [HttpGet("permissions")]
        public IActionResult GetAllPermissions()
        {
            return Ok(Permissions.All);
        }

        [HttpPut("{roleId}/permissions")]
        public async Task<IActionResult> UpdateRolePermissions
        (
            Guid roleId,
            UpdateRolePermissionsRequest payload
        )
        {
            var result = await roleService.UpdateRolePermissions(roleId, payload);

            return ToApiResult(result);
        }
    }
}

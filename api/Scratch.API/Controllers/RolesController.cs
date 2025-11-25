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
        public async Task<IActionResult> GetAllRoles([FromQuery] bool nameOnly)
        {
            if (nameOnly)
            {
                var nameResult = await roleService.GetAllName();
                return ToApiResult(nameResult);
            }

            var roleResult = await roleService.GetAll();
            return ToApiResult(roleResult);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleRequest payload)
        {
            var result = await roleService.Add(payload);
            
            return ToApiResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(Guid id, UpdateRoleRequest payload)
        {
            var result = await roleService.Update(id, payload);

            return ToApiResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var result = await roleService.Delete(id);

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

using Application.Authorization;
using Application.Interfaces.Services;
using Application.Models.Requests.Role;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/roles")]
    [RequirePermission(Permissions.DashboardAccess)]
    public class RolesController(IRoleService roleService) : BaseController
    {
        [HttpGet]
        [RequirePermission(Permissions.ManageRoles)]
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
        [RequirePermission(Permissions.ManageRoles)]
        public async Task<IActionResult> AddRole(AddRoleRequest payload)
        {
            var result = await roleService.Add(payload);
            return ToApiResult(result);
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.ManageRoles)]
        public async Task<IActionResult> UpdateRole(int id, UpdateRoleRequest payload)
        {
            var result = await roleService.Update(id, payload);
            return ToApiResult(result);
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.ManageRoles)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await roleService.Delete(id);
            return ToApiResult(result);
        }

        [HttpGet("permissions")]
        [RequirePermission(Permissions.ManageRoles)]
        public IActionResult GetAllPermissions()
        {
            return Ok(Permissions.All);
        }

        [HttpPut("{roleId}/permissions")]
        [RequirePermission(Permissions.ManageRoles)]
        public async Task<IActionResult> UpdateRolePermissions
        (
            int roleId,
            UpdateRolePermissionsRequest payload
        )
        {
            var result = await roleService.UpdateRolePermissions(roleId, payload);
            return ToApiResult(result);
        }
    }
}

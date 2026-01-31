using Scratch.Application.Models.Requests;
using Scratch.Application.Results;
using Scratch.Application.Models.Responses;

namespace Scratch.Application.Interfaces.Services
{
    public interface IRoleService
    {
        Task<Result<RoleDto[]>> GetAll();
        Task<Result<string[]>> GetAllName();
        Task<Result<RoleDto>> Add(AddRoleRequest payload);
        Task<Result> Update(int id, UpdateRoleRequest payload);
        Task<Result> Delete(int id);
        Task<Result> UpdateRolePermissions(int roleId, UpdateRolePermissionsRequest payload);
    }
}

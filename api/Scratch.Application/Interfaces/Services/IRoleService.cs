using Scratch.Domain.Dto;
using Scratch.Domain.Requests;
using Scratch.Domain.Results;

namespace Scratch.Application.Interfaces.Services
{
    public interface IRoleService
    {
        Task<Result<RoleDto[]>> GetAll();
        Task<Result<string[]>> GetAllName();
        Task<Result<RoleDto>> Add(AddRoleRequest payload);
        Task<Result> Update(Guid id, UpdateRoleRequest payload);
        Task<Result> Delete(Guid id);
        Task<Result> UpdateRolePermissions(Guid roleId, UpdateRolePermissionsRequest payload);
    }
}

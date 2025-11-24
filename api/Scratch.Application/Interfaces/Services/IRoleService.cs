using Scratch.Domain.Dto;
using Scratch.Domain.Requests;
using Scratch.Domain.Results;

namespace Scratch.Application.Interfaces.Services
{
    public interface IRoleService
    {
        Task<Result<RoleDto[]>> GetAll();
        Task<Result> UpdateRolePermissions(Guid roleId, UpdateRolePermissionsRequest payload);
    }
}

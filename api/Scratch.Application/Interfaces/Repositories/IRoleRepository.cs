using Scratch.Domain.Dto;
using Scratch.Domain.Entities;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<RoleDto[]> GetAll();
        Task<string[]> GetAllName();
        Task<bool> IsRoleExist(string name);
        Task<Role?> GetRoleById(Guid id);
        Task AddRole(Role role);
        Task DeleteRole(Guid id);
        Task AddPermissions(Guid roleId, string[] permissions);
        Task DeletePermissions(Guid roleId, string[]? permissions);
    }
}

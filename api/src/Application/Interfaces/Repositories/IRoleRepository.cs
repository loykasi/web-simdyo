using Application.Models.Responses;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<RoleDto[]> GetAll();
        Task<string[]> GetAllName();
        Task<bool> IsRoleExist(string name);
        Task<Role?> GetRoleById(int id);
        Task AddRole(Role role);
        Task DeleteRole(int id);
        Task AddPermissions(int roleId, string[] permissions);
        Task DeletePermissions(int roleId, string[]? permissions);
    }
}

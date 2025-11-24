using Scratch.Domain.Dto;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<RoleDto[]> GetAll();
        Task AddPermissions(Guid roleId, string[] permissions);
        Task DeletePermissions(Guid roleId, string[]? permissions);
    }
}

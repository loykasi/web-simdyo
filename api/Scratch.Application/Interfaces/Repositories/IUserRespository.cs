using Scratch.Domain.DTO;
using Scratch.Domain.Entities;
using Scratch.Domain.Responses;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IUserRespository
    {
        Task<Pagination<UserDto>> Get(int? pageNumber = null, int ? size = null);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<string[]> GetUserPermissionsAsync(User user);
        Task SetUserRole(User user, string[] roles);
    }
}

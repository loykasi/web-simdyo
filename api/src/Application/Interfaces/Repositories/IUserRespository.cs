using Application.Models.Responses;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IUserRespository
    {
        Task<Pagination<UserDto>> Get(string? searchTerm, int? pageNumber = null, int ? size = null);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<string[]> GetUserPermissionsAsync(User user);
        Task SetUserRole(User user, string[] roles);
    }
}

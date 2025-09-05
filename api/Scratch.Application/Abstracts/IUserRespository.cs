using Scratch.Domain.Entities;

namespace Scratch.Application.Abstracts
{
    public interface IUserRespository
    {
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    }
}

using Scratch.Domain.Entities;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface ICurrentUserService
    {
        bool HasValidAccessToken();
        string GetUserID();
        Task<User?> GetUserAsync();
    }
}

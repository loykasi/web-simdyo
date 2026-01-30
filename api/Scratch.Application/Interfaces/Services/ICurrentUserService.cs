using Scratch.Domain.Entities;

namespace Scratch.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        bool HasValidAccessToken();
        string GetUserID();
        Task<User?> GetUserAsync();
    }
}

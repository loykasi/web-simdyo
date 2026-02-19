using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        bool HasValidAccessToken();
        string GetUserID();
        Task<User?> GetUserAsync();
    }
}

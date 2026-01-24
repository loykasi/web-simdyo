using Scratch.Domain.Entities;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IUserBanRepository
    {
        Task<bool> GetBanStatus(int userId);
        Task<UserBan?> GetByUserId(int userId);
        void Add(UserBan userBan);
    }
}

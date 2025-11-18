using Scratch.Domain.Entities;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IUserBanRepository
    {
        Task<bool> GetBanStatus(Guid userId);
        Task<UserBan?> GetByUserId(Guid userId);
        void Add(UserBan userBan);
    }
}

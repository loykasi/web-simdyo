using Scratch.Domain.Entities;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IUserDailyUploadStatsRepository
    {
        Task<UserDailyUploadStats?> GetByUserIdAsync(int userId, DateOnly date);
        void Add(UserDailyUploadStats stat);
    }
}

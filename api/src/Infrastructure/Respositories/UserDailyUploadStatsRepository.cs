using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Respositories
{
    public class UserDailyUploadStatsRepository(ApplicationDbContext dbContext) : IUserDailyUploadStatsRepository
    {
        public async Task<UserDailyUploadStats?> GetByUserIdAsync(int userId, DateOnly date)
        {
            return await dbContext.UserDailyUploadStats
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Date == date);
        }

        public void Add(UserDailyUploadStats stat)
        {
            dbContext.UserDailyUploadStats.Add(stat);
        }
    }
}

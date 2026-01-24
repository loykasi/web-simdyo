using Microsoft.EntityFrameworkCore;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Entities;

namespace Scratch.Infrastructure.Respositories
{
    public class UserBanRepository(ApplicationDbContext dbContext) : IUserBanRepository
    {
        public async Task<bool> GetBanStatus(int userId)
        {
            return await dbContext.UserBans.AnyAsync(u => u.UserId == userId && u.IsActive == true);
        }

        public async Task<UserBan?> GetByUserId(int userId)
        {
            return await dbContext.UserBans.Where(u => u.UserId == userId && u.IsActive)
                                            .OrderByDescending(u => u.CreatedAt)
                                            .FirstOrDefaultAsync();
        }

        public void Add(UserBan userBan)
        {
            dbContext.Add(userBan);
        }
    }
}

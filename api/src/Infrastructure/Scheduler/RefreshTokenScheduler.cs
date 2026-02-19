using Application.Interfaces.Schedulers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Scheduler
{
    public class RefreshTokenScheduler(ApplicationDbContext dbContext) : IRefreshTokenScheduler
    {
        public async Task RemoveExpiredRefreshTokenAsync()
        {
            await dbContext.RefreshTokens
                .Where(x => x.RefreshTokenExpriresAtUTC < DateTime.UtcNow.AddDays(-7))
                .ExecuteDeleteAsync();
        }
    }
}

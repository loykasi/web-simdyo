using Microsoft.EntityFrameworkCore;
using Scratch.Application.Interfaces.Schedulers;

namespace Scratch.Infrastructure.Scheduler
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

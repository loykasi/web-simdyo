using Application.Interfaces.Schedulers;
using Hangfire;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Background
{
    public class HangfireRecurringJobService
    (
        IRecurringJobManager recurringJobManager
    ): IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            recurringJobManager.AddOrUpdate<IRefreshTokenScheduler>
            (
                "cleanup-refreshtoken",
                job => job.RemoveExpiredRefreshTokenAsync(),
                Cron.Daily
            );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

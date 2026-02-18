using Hangfire;
using Scratch.Application.Interfaces.Schedulers;

namespace Scratch.API.Extensions
{
    public static class HangfireJobRegistrar
    {
        public static void Register()
        {
            RecurringJob.AddOrUpdate<IRefreshTokenScheduler>
            (
                "cleanup-refreshtoken",
                job => job.RemoveExpiredRefreshTokenAsync(),
                Cron.Minutely
            );
        }
    }
}

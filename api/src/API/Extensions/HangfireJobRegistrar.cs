using Application.Interfaces.Schedulers;
using Hangfire;

namespace API.Extensions
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

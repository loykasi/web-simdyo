using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Scratch.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddBackgroundJobsServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(options =>
                    options.UseNpgsqlConnection(configuration.GetConnectionString("DbConnectionString"))
                )
            );

            services.AddHangfireServer();

            return services;
        }
    }
}

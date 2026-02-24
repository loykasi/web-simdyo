using Infrastructure.Options;

namespace API.Extensions;

public static class CorsExtension
{
    public static void SetupCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOptions = configuration.GetSection(CorsOptions.OptionsKey).Get<CorsOptions>()
                    ?? throw new ArgumentException(nameof(CorsOptions));

        services.AddCors(options =>
        {
            options.AddPolicy
            (
                "AllowOrigins",
                policy =>
                {
                    policy.WithOrigins(corsOptions.AllowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }
            );
        });
    }
}
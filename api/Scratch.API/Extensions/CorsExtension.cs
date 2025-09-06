namespace Scratch.API.Extensions;

public static class CorsExtension
{
    public static void SetupCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy
            (
                "AllowOrigins",
                policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }
            );
        });
    }
}
using API.Extensions;
using API.Handlers;
using Hangfire;
using Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOptions();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString"));
});

builder.Services.AddIdentity();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorization(builder.Configuration);
builder.Services.SetupCors(builder.Configuration);
builder.Services.AddServices();

builder.Services.AddBackgroundJobsServices(builder.Configuration);

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>
    (
        httpContext => RateLimitPartition.GetFixedWindowLimiter
        (
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }
        )
    );

    options.AddFixedWindowLimiter("login", cfg =>
    {
        cfg.PermitLimit = 5;
        cfg.Window = TimeSpan.FromMinutes(1);
        cfg.QueueLimit = 0;
    });

    options.AddFixedWindowLimiter("register", cfg =>
    {
        cfg.PermitLimit = 8;
        cfg.Window = TimeSpan.FromMinutes(5);
    });

    options.AddTokenBucketLimiter("comment", cfg =>
    {
        cfg.TokenLimit = 5;
        cfg.QueueLimit = 0;
        cfg.TokensPerPeriod = 1;
        cfg.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
        cfg.AutoReplenishment = true;
    });

    //options.AddSlidingWindowLimiter("comment", cfg =>
    //{
    //    cfg.PermitLimit = 10;
    //    cfg.Window = TimeSpan.FromMinutes(1);
    //    cfg.SegmentsPerWindow = 5;
    //});

    //options.AddFixedWindowLimiter("fixed", cfg =>
    //{
    //    cfg.PermitLimit = 1;
    //    cfg.Window = TimeSpan.FromSeconds(1);
    //});

    //options.AddPolicy("per-user", httpContext =>
    //{
    //    string? userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

    //    if (!string.IsNullOrWhiteSpace(userId))
    //    {
    //        return RateLimitPartition.GetTokenBucketLimiter(
    //            userId,
    //            _ => new TokenBucketRateLimiterOptions
    //            {
    //                TokenLimit = 50,
    //                TokensPerPeriod = 10,
    //                ReplenishmentPeriod = TimeSpan.FromSeconds(1),
    //            });
    //    }

    //    return RateLimitPartition.GetFixedWindowLimiter(
    //        "anonymous",
    //        _ => new FixedWindowRateLimiterOptions
    //        {
    //            PermitLimit = 5,
    //            Window = TimeSpan.FromSeconds(1)
    //        });
    //});
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false)
        );
    });

builder.Services.AddOpenApi();

var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     db.Database.Migrate();
// }

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("API");
    });

    app.UseHangfireDashboard(options: new DashboardOptions
    {
        Authorization = [],
        DarkModeEnabled = true
    });

    await app.Seed();
}

app.UseCors("AllowOrigins");
app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

HangfireJobRegistrar.Register();

app.Run();

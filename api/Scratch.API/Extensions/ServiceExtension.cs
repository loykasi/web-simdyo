using Scratch.Application.Abstracts;
using Scratch.Application.Services;
using Scratch.Infrastructure.Processors;
using Scratch.Infrastructure.Respositories;
using Scratch.Infrastructure.Services;

namespace Scratch.API.Extensions;
public static class ServiceExtension
{
    public static void SetupServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthTokenProcessor, AuthTokenProcessor>();
        services.AddScoped<IUserRespository, UserRespository>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
}

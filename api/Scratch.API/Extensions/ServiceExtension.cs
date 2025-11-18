using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Application.Interfaces.Services;
using Scratch.Application.Services;
using Scratch.Infrastructure;
using Scratch.Infrastructure.Processors;
using Scratch.Infrastructure.Respositories;
using Scratch.Infrastructure.Services;

namespace Scratch.API.Extensions;
public static class ServiceExtension
{
    public static void SetupServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        //services.AddScoped<IUserRespository, UserRespository>();
        //services.AddScoped<IProjectRepository, ProjectRespository>();

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IProjectLikeService, ProjectLikeService>();
        services.AddScoped<IProjectCommentService, ProjectCommentService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IUserBanService, UserBanService>();
        services.AddScoped<IProjectReportService, ProjectReportService>();

        services.AddScoped<IAuthTokenProcessor, AuthTokenProcessor>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddSingleton<IObjectStorageService, ObjectStorageService>();
        services.AddSingleton<IPublicIdService, PublicIdService>();
    }
}

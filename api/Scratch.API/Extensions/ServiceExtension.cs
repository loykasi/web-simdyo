using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Services;
using Scratch.Application.Services;
using Scratch.Infrastructure;
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

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IProjectReactionService, ProjectReactionService>();
        services.AddScoped<IProjectCommentService, ProjectCommentService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserBanService, UserBanService>();
        services.AddScoped<IProjectReportService, ProjectReportService>();
        services.AddScoped<IProjectBanService, ProjectBanService>();
        services.AddScoped<IProjectCategoryService, ProjectCategoryService>();

        services.AddScoped<IAuthTokenService, AuthTokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPublicIdService, PublicIdService>();

        services.AddSingleton<IObjectStorageService, ObjectStorageService>();
    }
}

using Microsoft.AspNetCore.Authentication;
using Scratch.Application.Authorization;
using Scratch.Domain.Authorizations;

namespace Scratch.API.Extensions;

public static class AuthorizationExtension
{
    public static void AddAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        services.AddTransient<IClaimsTransformation, PermissionClaimsTransformation>();

        //services.AddAuthorizationBuilder()
        //    .AddPolicy(Permissions.ProjectsRead, policy => policy.RequirePermission(Permissions.ProjectsRead))
        //    .AddPolicy(Permissions.ProjectsUpdate, policy => policy.RequirePermission(Permissions.ProjectsUpdate))
        //    .AddPolicy(Permissions.ProjectsDelete, policy => policy.RequirePermission(Permissions.ProjectsDelete));
    }
}
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Scratch.Application.Authorization;
using Scratch.Domain.Authorizations;

namespace Scratch.API.Extensions;

public static class AuthorizationExtension
{
    public static void AddAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddTransient<IClaimsTransformation, PermissionClaimsTransformation>();

        //services.AddAuthorizationBuilder()
        //    .AddPolicy(Permissions.DashboardAccess, policy => policy.RequirePermission(Permissions.DashboardAccess))
        //    .AddPolicy(Permissions.ManageUsers, policy => policy.RequirePermission(Permissions.ManageUsers))
        //    .AddPolicy(Permissions.ManageProjects, policy => policy.RequirePermission(Permissions.ManageProjects))
        //    .AddPolicy(Permissions.ManageProjectReport, policy => policy.RequirePermission(Permissions.ManageProjectReport))
        //    .AddPolicy(Permissions.ManageCategories, policy => policy.RequirePermission(Permissions.ManageCategories))
        //    .AddPolicy(Permissions.ManageRoles, policy => policy.RequirePermission(Permissions.ManageRoles));
    }
}
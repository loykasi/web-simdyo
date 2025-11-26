using Microsoft.AspNetCore.Authorization;

namespace Scratch.Application.Authorization
{
    public static class PermissionExtension
    {
        public static void RequirePermission(
            this AuthorizationPolicyBuilder builder,
            params string[] permissions)
        {
            builder.AddRequirements(new PermissionAuthorizationRequirement(permissions));
        }
    }
}

using Microsoft.AspNetCore.Authorization;

namespace Scratch.Application.Authorization
{
    public static class PermissionExtension
    {
        public static void RequirePermission(
            this AuthorizationPolicyBuilder builder,
            params string[] permission)
        {
            builder.AddRequirements(new PermissionAuthorizationRequirement(permission));
        }
    }
}

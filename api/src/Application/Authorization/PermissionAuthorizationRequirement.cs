using Domain.Options;
using Microsoft.AspNetCore.Authorization;

namespace Application.Authorization
{
    public class PermissionAuthorizationRequirement(params string[] allowedPermissions) 
        : AuthorizationHandler<PermissionAuthorizationRequirement>, IAuthorizationRequirement
    {
        public string[] AllowedPermissions { get; } = allowedPermissions;

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionAuthorizationRequirement requirement)
        {
            foreach (var permission in requirement.AllowedPermissions)
            {
                bool found = context.User.FindFirst(c =>
                    c.Type == CustomClaimTypes.Permission &&
                    c.Value == permission) is not null;

                // only allow dashboard access if user logged with account's password
                if (permission == Permissions.DashboardAccess)
                {
                    var isUseOTPClaim = context.User.FindFirst(CustomClaimTypes.IsUseOTP)?.Value;
                    if (bool.TryParse(isUseOTPClaim, out bool isUseOTP))
                    {
                        found = found && !isUseOTP;
                    }
                    else
                    {
                        found = false;
                    }
                }

                if (found)
                {
                    context.Succeed(requirement);
                    break;
                }
            }
            return Task.CompletedTask;
        }
    }
}

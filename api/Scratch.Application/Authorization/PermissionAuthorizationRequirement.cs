using Microsoft.AspNetCore.Authorization;
using Scratch.Domain.Authorizations;

namespace Scratch.Application.Authorization
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
                    c.Type == CustomClaimType.Permission &&
                    c.Value == permission) is not null;

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

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Scratch.Application.Authorization
{
    public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith("PERMISSION:"))
            {
                return await base.GetPolicyAsync(policyName);
            }

            var permissions = GetPermissionsFromPolicy(policyName);

            return new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionAuthorizationRequirement(permissions))
                .Build();
        }

        private static string[] GetPermissionsFromPolicy(string policyName)
        {
            return policyName
                .Replace("PERMISSION:", null)
                .Split(",", StringSplitOptions.RemoveEmptyEntries);
        }
    }
}

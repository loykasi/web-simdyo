using Microsoft.AspNetCore.Authorization;

namespace Scratch.Application.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RequirePermissionAttribute : AuthorizeAttribute
    {
        public RequirePermissionAttribute(params string[] permissions)
            : base(policy: $"PERMISSION:{string.Join(",", permissions)}")
        { }
    }
}

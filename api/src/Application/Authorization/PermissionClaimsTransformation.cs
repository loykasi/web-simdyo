using Application.Interfaces.Services;
using Domain.Options;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Application.Authorization
{
    public class PermissionClaimsTransformation(IPermissionService permissionService) : IClaimsTransformation
    {
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity?.IsAuthenticated != true)
            {
                return principal;
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return principal;
            }

            var permissions = await permissionService.GetUserPermissionsAsync(userId);

            if (!permissions.IsSuccess)
            {
                return principal;
            }

            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            foreach (var permission in permissions.Value!)
            {
                claimsIdentity.AddClaim(new Claim(CustomClaimTypes.Permission, permission));
            }

            return principal;
        }
    }
}

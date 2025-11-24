using Microsoft.EntityFrameworkCore;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Authorizations;
using Scratch.Domain.Dto;
using System.Security.Claims;

namespace Scratch.Infrastructure.Respositories
{
    public class RoleRepository
    (
        ApplicationDbContext dbContext
    ) : IRoleRepository
    {
        public async Task<RoleDto[]> GetAll()
        {
            return await dbContext.Roles
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name!,
                    Permissions = dbContext.RoleClaims.Where(c => c.RoleId == r.Id).Select(c => c.ClaimValue!).ToArray()
                })
                .ToArrayAsync();
        }

        public async Task AddPermissions(Guid roleId, string[] permissions)
        {
            var existingPermissions = await dbContext.RoleClaims
                .Where(c => c.RoleId == roleId)
                .Select(c => c.ClaimValue)
                .ToListAsync();

            var claims = permissions
                .Where(p => !existingPermissions.Contains(p) && Permissions.All.Contains(p))
                .Select(p => new Microsoft.AspNetCore.Identity.IdentityRoleClaim<Guid>()
                {
                    RoleId = roleId,
                    ClaimType = CustomClaimType.Permission,
                    ClaimValue = p
                });
            dbContext.RoleClaims.AddRange(claims);
        }

        public async Task DeletePermissions(Guid roleId, string[]? permissions)
        {
            if (permissions == null || permissions.Length == 0)
            {
                return;
            }

            await dbContext.RoleClaims
                .Where(c => c.RoleId == roleId && permissions.Contains(c.ClaimValue))
                .ExecuteDeleteAsync();
        }
    }
}

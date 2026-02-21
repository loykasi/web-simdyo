using Application.Authorization;
using Application.Interfaces.Repositories;
using Application.Models.Responses.Role;
using Domain.Entities;
using Domain.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Respositories
{
    public class RoleRepository
    (
        ApplicationDbContext dbContext,
        RoleManager<Role> roleManager
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

        public async Task<string[]> GetAllName()
        {
            return await dbContext.Roles
                .Select(r => r.Name!)
                .ToArrayAsync();
        }

        public async Task<bool> IsRoleExist(string name)
        {
            return await dbContext.Roles.AnyAsync(r => r.Name!.Equals(name));
        }

        public async Task<Role?> GetRoleById(int id)
        {
            return await dbContext.Roles.Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddRole(Role role)
        {
            await roleManager.CreateAsync(role);
            //dbContext.Roles.Add(role);
        }

        public async Task DeleteRole(int id)
        {
            await dbContext.Roles.Where(r => r.Id == id).ExecuteDeleteAsync();
        }

        public async Task AddPermissions(int roleId, string[] permissions)
        {
            var existingPermissions = await dbContext.RoleClaims
                .Where(c => c.RoleId == roleId)
                .Select(c => c.ClaimValue)
                .ToListAsync();

            var claims = permissions
                .Where(p => !existingPermissions.Contains(p) && Permissions.All.Contains(p))
                .Select(p => new IdentityRoleClaim<int>()
                {
                    RoleId = roleId,
                    ClaimType = CustomClaimTypes.Permission,
                    ClaimValue = p
                });
            dbContext.RoleClaims.AddRange(claims);
        }

        public async Task DeletePermissions(int roleId, string[]? permissions)
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

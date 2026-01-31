using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scratch.Application.Authorization;
using Scratch.Domain.Entities;
using Scratch.Domain.Options;
using System.Security.Claims;

namespace Scratch.Infrastructure
{
    public static class Seeder
    {
        public static async Task Seed(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            
            await SeedRolesAndPermissions(roleManager);
            await SeedAdmin(userManager);
            await FixNullSecurityStamps(userManager);
        }

        public static async Task SeedRolesAndPermissions(RoleManager<Role> roleManager)
        {
            var memberRole = await roleManager.FindByNameAsync(Roles.Member);
            if (memberRole is null)
            {
                await roleManager.CreateAsync(memberRole = new Role(Roles.Member));
            }


            var adminRole = await roleManager.FindByNameAsync(Roles.Admin);
            if (adminRole is null)
            {
                await roleManager.CreateAsync(adminRole = new Role(Roles.Admin));
            }

            var claims = await roleManager.GetClaimsAsync(adminRole);
            foreach (var permission in Permissions.All)
            {
                await AddPermissionClaim(roleManager, claims, adminRole, permission);
            }
        }

        private static async Task AddPermissionClaim
        (
            RoleManager<Role> roleManager,
            IList<Claim>? claims,
            Role role,
            string permission
        )
        {
            if (claims == null || !claims.Any(c => c.Value.Equals(permission)))
            {
                var claim = new Claim(CustomClaimTypes.Permission, permission);
                await roleManager.AddClaimAsync(
                    role,
                    claim
                );
            }
        }

        public static async Task SeedAdmin(UserManager<User> userManager)
        {
            var adminEmail = "admin@gmail.com";
            var adminPassword = "123456";

            var userExist = await userManager.FindByEmailAsync(adminEmail);
            if (userExist == null)
            {
                var adminUser = new User
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Roles.Admin);
                }
                else
                {
                    throw new Exception("Failed to create the admin user: " + string.Join(", ", result.Errors));
                }
            }
        }

        public static async Task FixNullSecurityStamps(UserManager<User> userManager)
        {
            var users = await userManager.Users
                .Where(u => u.SecurityStamp == null)
                .ToListAsync();

            foreach (var user in users)
            {
                await userManager.UpdateSecurityStampAsync(user);
            }
        }
    }
}

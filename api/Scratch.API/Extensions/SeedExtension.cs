using Microsoft.AspNetCore.Identity;
using Scratch.Domain.Authorizations;
using Scratch.Domain.Entities;
using System.Security.Claims;

namespace Scratch.API.Extensions
{
    public static class SeedExtension
    {
        public static async Task SeedRolesAndPermissions(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

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
                var claim = new Claim(CustomClaimType.Permission, permission);
                await roleManager.AddClaimAsync(
                    role,
                    claim
                );
            }
        }

        public static async Task SeedAdmin(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

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
    }
}

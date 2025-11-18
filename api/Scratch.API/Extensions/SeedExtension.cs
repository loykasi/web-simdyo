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

                await roleManager.AddClaimAsync(
                    adminRole,
                    new Claim(CustomClaimType.Permission, Permissions.ProjectsRead)
                );

                await roleManager.AddClaimAsync(
                    adminRole,
                    new Claim(CustomClaimType.Permission, Permissions.ProjectsUpdate)
                );

                await roleManager.AddClaimAsync(
                    adminRole,
                    new Claim(CustomClaimType.Permission, Permissions.ProjectsDelete)
                );
            }

            var memberRole = await roleManager.FindByNameAsync(Roles.Member);
            if (memberRole is null)
            {
                await roleManager.CreateAsync(memberRole = new Role(Roles.Member));

                await roleManager.AddClaimAsync(
                    memberRole,
                    new Claim(CustomClaimType.Permission, Permissions.ProjectsRead)
                );

                await roleManager.AddClaimAsync(
                    memberRole,
                    new Claim(CustomClaimType.Permission, Permissions.ProjectsUpdate)
                );

                await roleManager.AddClaimAsync(
                    memberRole,
                    new Claim(CustomClaimType.Permission, Permissions.ProjectsDelete)
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

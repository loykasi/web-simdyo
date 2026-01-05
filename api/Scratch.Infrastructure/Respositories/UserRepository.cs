using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Authorizations;
using Scratch.Domain.DTO;
using Scratch.Domain.Entities;
using System.Data;

namespace Scratch.Infrastructure.Respositories
{
    public class UserRepository(
        ApplicationDbContext dbContext,
        UserManager<User> userManager
    ) : IUserRespository
    {
        public async Task<Pagination<UserDto>> Get(
            string? searchTerm,
            int? pageNumber = null,
            int? size = null
        )
        {
            int pageSize = size ?? 10;
            int currentPage = pageNumber ?? 1;
            int offset = (currentPage - 1) * pageSize;

            string search = (searchTerm ?? string.Empty).ToLower();

            var query = dbContext.Users
                .Where(u =>
                    u.UserName!.ToLower().Contains(search) ||
                    u.Email!.ToLower().Contains(search)
                )
                .OrderBy(u => u.CreatedAt);

            var users = await query
                .Skip(offset)
                .Take(pageSize)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.UserName!,
                    Email = u.Email!,
                    CreatedAt = u.CreatedAt.ToString("o"),
                    Roles = (from userRole in dbContext.UserRoles
                                join role in dbContext.Roles on userRole.RoleId equals role.Id
                                where userRole.UserId == u.Id
                                select role.Name).ToArray(),
                    IsBanned = (dbContext.UserBans.Any(b => b.UserId == u.Id && b.IsActive == true))
                })
                .ToListAsync();

            int count = await query.CountAsync();

            Pagination<UserDto> pagination = new()
            {
                Total = count,
                Size = users.Count,
                Items = users
            };
            return pagination;
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            //var user = await dbContext.Users.FirstOrDefaultAsync(u =>
            //    u.RefreshToken != null &&
            //    u.RefreshToken.Equals(refreshToken)
            //);

            var user = await dbContext.Users
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

            return user;
        }

        public async Task<string[]> GetUserPermissionsAsync(User user)
        {
            var roles = await userManager.GetRolesAsync(user);

            var permissions = await (
                from role in dbContext.Roles
                join claims in dbContext.RoleClaims on role.Id equals claims.RoleId
                where roles.Contains(role.Name!) && 
                      claims.ClaimType == CustomClaimType.Permission
                select claims.ClaimValue)
            .Distinct()
            .ToArrayAsync();

            return permissions;
        }

        public async Task SetUserRole(User user, string[] roles)
        {
            //await dbContext.UserRoles.Where(u => u.UserId == user.Id).ExecuteDeleteAsync();

            var currentRoles = await userManager.GetRolesAsync(user);
            
            await userManager.RemoveFromRolesAsync(user, currentRoles);
            await userManager.AddToRolesAsync(user, roles);

            foreach (var item in roles)
            {
                Console.WriteLine(item);
            }
        }
    }
}

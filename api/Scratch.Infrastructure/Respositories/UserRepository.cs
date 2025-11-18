using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Authorizations;
using Scratch.Domain.DTO;
using Scratch.Domain.Entities;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;
using System.Data;

namespace Scratch.Infrastructure.Respositories
{
    public class UserRepository(
        ApplicationDbContext dbContext,
        UserManager<User> userManager
    ) : IUserRespository
    {
        public async Task<Pagination<UserDto>> Get(int? pageNumber = null, int? size = null)
        {
            int pageSize = size ?? 10;
            int currentPage = pageNumber ?? 1;
            int offset = (currentPage - 1) * pageSize;

            var users = await dbContext.Users.OrderBy(u => u.CreatedAt)
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

            int count = await dbContext.Users.Select(p => p.Id).CountAsync();

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
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

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
    }
}

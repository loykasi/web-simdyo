using Microsoft.EntityFrameworkCore;
using Scratch.Application.Abstracts;
using Scratch.Domain.Entities;

namespace Scratch.Infrastructure.Respositories
{
    public class UserRepository(ApplicationDbContext applicationDbContext): IUserRespository
    {
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            return user;
        }
    }
}

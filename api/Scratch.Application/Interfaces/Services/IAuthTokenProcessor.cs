using Scratch.Domain.Entities;
using System.Security.Claims;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IAuthTokenProcessor
    {
        Task<(string token, DateTime expiresAtUTC)> GenerateJwtToken(User user);
        RefreshToken? GetRefreshTokenByUser(User user, string refreshToken);
        RefreshToken GenerateRefreshToken(User user);
        RefreshToken GenerateRefreshToken(User user, string refreshToken);
        void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration);
        ClaimsPrincipal ValidateToken(string token);
        Task RevokeToken(User user, string refreshToken);
    }
}

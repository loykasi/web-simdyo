using Domain.Entities;
using System.Security.Claims;

namespace Application.Interfaces.Services
{
    public interface IAuthTokenService
    {
        Task<(string token, DateTime expiresAtUTC)> GenerateJwtToken(User user, bool isUseOTP = false);
        RefreshToken? GetRefreshTokenByUser(User user, string refreshToken);
        RefreshToken GenerateRefreshToken(User user);
        RefreshToken GenerateRefreshToken(User user, string refreshToken);
        void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration);
        ClaimsPrincipal ValidateToken(string token);
        Task RevokeToken(User user, string refreshToken);
    }
}

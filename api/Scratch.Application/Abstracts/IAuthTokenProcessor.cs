using Scratch.Domain.Entities;
using System.Security.Claims;

namespace Scratch.Application.Abstracts
{
    public interface IAuthTokenProcessor
    {
        (string token, DateTime expiresAtUTC) GenerateJwtToken(User user);
        string GenerateRefreshToken();
        void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration);
        ClaimsPrincipal ValidateToken(string token);
    }
}

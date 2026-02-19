using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Options;
using Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services;

public class AuthTokenService
(
    IOptions<JwtOptions> options,
    IHttpContextAccessor httpContextAccessor,
    UserManager<User> userManager,
    ApplicationDbContext dbContext
) : IAuthTokenService
{
    private readonly JwtOptions _jwtOptions = options.Value;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<(string token, DateTime expiresAtUTC)> GenerateJwtToken(User user, bool isUseOTP = false)
    {
        var signingKey = new SymmetricSecurityKey
        (
            Encoding.UTF8.GetBytes(_jwtOptions.Secret)
        );

        var credentials = new SigningCredentials
        (
            signingKey,
            SecurityAlgorithms.HmacSha256
        );

        var roles = await userManager.GetRolesAsync(user);

        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            ..roles.Select(r => new Claim(ClaimTypes.Role, r)),
            new Claim(CustomClaimTypes.IsUseOTP, isUseOTP.ToString(), ClaimValueTypes.Boolean)
        ];

        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationTimeInMinutes);

        var token = new JwtSecurityToken
        (
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return (jwtToken, expires);
    }

    public RefreshToken? GetRefreshTokenByUser(User user, string refreshToken)
    {
        return dbContext.RefreshTokens
            .Where(r => r.UserId == user.Id && r.Token == refreshToken)
            .FirstOrDefault();
    }

    public RefreshToken GenerateRefreshToken(User user)
    {
        var randomNumber = new byte[64];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);

        RefreshToken refreshToken = new()
        {
            Token = Convert.ToBase64String(randomNumber),
            RefreshTokenExpriresAtUTC = DateTime.UtcNow.AddDays(7),
            User = user
        };
        return refreshToken;
    }

    public RefreshToken GenerateRefreshToken(User user, string refreshToken)
    {
        var refreshTokenModel = dbContext.RefreshTokens
            .Where(r => r.UserId == user.Id && r.Token == refreshToken)
            .FirstOrDefault();
        //var refreshTokenModel = user.RefreshTokens.SingleOrDefault(r => r.Token == refreshToken);

        var randomNumber = new byte[64];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);

        if (refreshTokenModel == null)
        {
            refreshTokenModel = new()
            {
                Token = Convert.ToBase64String(randomNumber),
                RefreshTokenExpriresAtUTC = DateTime.UtcNow.AddDays(7),
                User = user
            };
            user.RefreshTokens.Add(refreshTokenModel);

            return refreshTokenModel;
        }

        refreshTokenModel.Token = Convert.ToBase64String(randomNumber);
        refreshTokenModel.RefreshTokenExpriresAtUTC = DateTime.UtcNow.AddDays(7);

        return refreshTokenModel;
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        TokenValidationParameters validationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience
        };

        var claims = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);
        return claims;
    }

    public void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration)
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Append
        (
            cookieName,
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Expires = expiration,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            }
        );
    }

    public async Task RevokeToken(User user, string refreshToken)
    {
        await dbContext.RefreshTokens
            .Where(r => r.UserId == user.Id && r.Token == refreshToken)
            .ExecuteDeleteAsync();
    }
}

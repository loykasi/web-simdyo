using Microsoft.AspNetCore.Http;
using Scratch.Application.Interfaces.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Scratch.Infrastructure.Services
{
    public class CurrentUserService
    (
        ICookieService cookieService,
        IAuthTokenProcessor authTokenProcessor
    ) : ICurrentUserService
    {
        private readonly ICookieService _cookieService = cookieService;
        private readonly IAuthTokenProcessor _authTokenProcessor = authTokenProcessor;

        public bool HasValidAccessToken()
        {
            string token = _cookieService.Get("ACCESS_TOKEN");

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            try
            {
                ClaimsPrincipal claims = _authTokenProcessor.ValidateToken(token);
                return claims.Identity.IsAuthenticated;
            }
            catch
            {
                return false;
            }
        }

        public string GetUserID()
        {
            string token = _cookieService.Get("ACCESS_TOKEN");
            token ??= "";
            ClaimsPrincipal claims = _authTokenProcessor.ValidateToken(token);
            string id = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return id;
        }
    }
}

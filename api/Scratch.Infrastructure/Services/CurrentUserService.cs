using Microsoft.AspNetCore.Http;
using Scratch.Application.Abstracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Scratch.Infrastructure.Services
{
    public class CurrentUserService
    (
        ITokenService tokenService,
        IAuthTokenProcessor authTokenProcessor
    ) : ICurrentUserService
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly IAuthTokenProcessor _authTokenProcessor = authTokenProcessor;

        public string GetUserID()
        {
            string token = _tokenService.Get("ACCESS_TOKEN");
            ClaimsPrincipal claims = _authTokenProcessor.ValidateToken(token);
            string id = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return id;
        }
    }
}

using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class CurrentUserService
    (
        ICookieService cookieService,
        IAuthTokenService authTokenProcessor,
        UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor
    ) : ICurrentUserService
    {
        private readonly ICookieService _cookieService = cookieService;
        private readonly IAuthTokenService _authTokenProcessor = authTokenProcessor;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

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
            if (string.IsNullOrEmpty(token))
            {
                return string.Empty;
            }

            try
            {
                ClaimsPrincipal claims = _authTokenProcessor.ValidateToken(token);
                return claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<User?> GetUserAsync()
        {
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        }
    }
}

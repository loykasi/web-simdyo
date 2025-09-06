using Microsoft.AspNetCore.Http;
using Scratch.Application.Abstracts;

namespace Scratch.Infrastructure.Services
{
    public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string Get(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[key];
        }

        public void Delete(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

        public void SetToken(string key, string value, DateTime? expiration = null)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            if (expiration != null )
            {
                options.Expires = expiration;
            }

            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, options);
        }
    }
}

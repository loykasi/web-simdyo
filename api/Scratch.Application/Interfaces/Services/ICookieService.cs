namespace Scratch.Application.Interfaces.Services
{
    public interface ICookieService
    {
        void SetToken(string key, string value, DateTime? expiration = null);
        string Get(string key);
        void Delete(string key);
    }
}

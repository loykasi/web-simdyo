namespace Scratch.Application.Interfaces.Repositories
{
    public interface ICookieService
    {
        void SetToken(string key, string value, DateTime? expiration = null);
        string Get(string key);
        void Delete(string key);
    }
}

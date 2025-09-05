namespace Scratch.Application.Abstracts
{
    public interface ITokenService
    {
        void SetToken(string key, string value, DateTime? expiration = null);
        string Get(string key);
    }
}

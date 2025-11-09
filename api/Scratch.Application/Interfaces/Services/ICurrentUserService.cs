namespace Scratch.Application.Interfaces.Repositories
{
    public interface ICurrentUserService
    {
        bool HasValidAccessToken();
        string GetUserID();
    }
}

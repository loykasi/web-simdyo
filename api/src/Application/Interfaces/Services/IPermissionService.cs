using Application.Results;

namespace Application.Interfaces.Services
{
    public interface IPermissionService
    {
        Task<Result<string[]>> GetUserPermissionsAsync(string userId);
    }
}

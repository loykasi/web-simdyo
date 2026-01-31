using Scratch.Application.Results;

namespace Scratch.Application.Interfaces.Services
{
    public interface IPermissionService
    {
        Task<Result<string[]>> GetUserPermissionsAsync(string userId);
    }
}

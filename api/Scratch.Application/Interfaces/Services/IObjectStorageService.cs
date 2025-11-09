using Microsoft.AspNetCore.Http;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IObjectStorageService
    {
        Task<string> Save(string name, IFormFile file);
    }
}

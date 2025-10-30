using Microsoft.AspNetCore.Http;

namespace Scratch.Application.Abstracts
{
    public interface IObjectStorageService
    {
        Task<string> Save(string name, IFormFile file);
    }
}

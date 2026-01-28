using Microsoft.AspNetCore.Http;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IObjectStorageService
    {
        Task<string> Save(string name, IFormFile file);
        string GetPath(string name);
        bool TryGetPreSignedUrl(string name, string contentType, string contentLength, out string preSignedUrl);
    }
}

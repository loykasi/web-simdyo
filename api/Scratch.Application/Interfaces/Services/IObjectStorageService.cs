using Microsoft.AspNetCore.Http;

namespace Scratch.Application.Interfaces.Services
{
    public interface IObjectStorageService
    {
        Task<string> Save(string name, IFormFile file);
        void DeleteJobs(IEnumerable<string> names);
        Task<bool> Delete(IEnumerable<string> names);
        string GetPath(string name);
        bool TryGetPreSignedUrl(string name, string contentType, long contentLength, out string preSignedUrl);
    }
}

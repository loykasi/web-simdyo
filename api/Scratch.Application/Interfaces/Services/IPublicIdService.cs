using Scratch.Domain.Entities;

namespace Scratch.Application.Interfaces.Services
{
    public interface IPublicIdService
    {
        string Encode(params int[] numbers);
        IReadOnlyList<int> Decode(string id);
        bool TryDecodeId(string publicId, out int id);
        Task<Project?> GetProject(string publicId);
    }
}

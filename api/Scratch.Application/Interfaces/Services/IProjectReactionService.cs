using Scratch.Application.Models.Requests;
using Scratch.Application.Results;

namespace Scratch.Application.Interfaces.Services
{
    public interface IProjectReactionService
    {
        //Task<Result<ProjectReactionResponse>> GetReactionCount(string publicId);
        Task<Result<string>> GetReactionStatus(string publicId);
        Task<Result> AddReaction(string publicId, AddReactionRequest request);
        Task<Result> DeleteReaction(string publicId);
    }
}

using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IProjectLikeService
    {
        Task<Result<int>> GetLike(string projectPublicId);
        Task<Result<bool>> GetLikeStatus(string projectPublicId);
        Task<Result> AddLike(string projectPublicId);
        Task<Result> DeleteLike(string projectPublicId);
    }
}

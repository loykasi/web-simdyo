using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.Application.Abstracts
{
    public interface IProjectCommentService
    {
        Task<Result<List<ProjectCommentResponse>>> GetComments(string projectPublicId, int? limit = null, int? lastId = null, int? parentId = null);
        Task<Result<ProjectCommentResponse>> Add(string projectPublicId, AddCommentRequest addCommentRequest);
        Task<Result> Remove(string projectPublicId, int commentId);
    }
}

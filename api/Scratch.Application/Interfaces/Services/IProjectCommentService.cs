using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IProjectCommentService
    {
        Task<Result<Pagination<ProjectCommentResponse>>> GetComments(string projectPublicId, int? limit = null, int? lastId = null, int? parentId = null);
        Task<Result<ProjectCommentResponse>> Add(string projectPublicId, AddCommentRequest addCommentRequest);
        Task<Result> Remove(string projectPublicId, int commentId);
    }
}

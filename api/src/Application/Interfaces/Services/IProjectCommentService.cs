using Application.Results;
using Domain.Entities;
using Application.Models.Requests.ProjectComment;
using Application.Models.Responses.ProjectComment;

namespace Application.Interfaces.Services
{
    public interface IProjectCommentService
    {
        Task<Result<Pagination<ProjectCommentResponse>>> GetComments(string projectPublicId, int? limit = null, int? lastId = null, int? parentId = null);
        Task<Result<ProjectCommentResponse>> Add(string projectPublicId, AddCommentRequest addCommentRequest);
        Task<Result> Remove(string projectPublicId, int commentId);
    }
}

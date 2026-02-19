using Application.Results;
using Application.Models.Requests;
using Application.Models.Responses;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IProjectCommentService
    {
        Task<Result<Pagination<ProjectCommentResponse>>> GetComments(string projectPublicId, int? limit = null, int? lastId = null, int? parentId = null);
        Task<Result<ProjectCommentResponse>> Add(string projectPublicId, AddCommentRequest addCommentRequest);
        Task<Result> Remove(string projectPublicId, int commentId);
    }
}

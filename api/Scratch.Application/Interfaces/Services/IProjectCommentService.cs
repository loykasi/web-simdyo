using Scratch.Domain.Entities;
using Scratch.Application.Results;
using Scratch.Application.Models.Requests;
using Scratch.Application.Models.Responses;

namespace Scratch.Application.Interfaces.Services
{
    public interface IProjectCommentService
    {
        Task<Result<Pagination<ProjectCommentResponse>>> GetComments(string projectPublicId, int? limit = null, int? lastId = null, int? parentId = null);
        Task<Result<ProjectCommentResponse>> Add(string projectPublicId, AddCommentRequest addCommentRequest);
        Task<Result> Remove(string projectPublicId, int commentId);
    }
}

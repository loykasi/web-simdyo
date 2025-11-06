using Scratch.Domain.Entities;
using Scratch.Domain.Responses;

namespace Scratch.Application.Abstracts
{
    public interface IProjectCommentRepository
    {
        Task<List<ProjectCommentResponse>> GetComments(int projectId, int? limit = null, int? lastId = null, int? ParentId = null);
        Task<ProjectComment> Get(int id);
        void Add(ProjectComment projectComment);
        Task Delete(int id);
    }
}

using Application.Models.Responses.ProjectComment;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IProjectCommentRepository
    {
        Task<int> Count();
        Task<Pagination<ProjectCommentResponse>> GetComments(int projectId, int? limit = null, int? lastId = null, int? ParentId = null);
        Task<ProjectComment> Get(int id);
        void Add(ProjectComment projectComment);
        Task Delete(int id);
    }
}

using Scratch.Domain.Entities;
using Scratch.Domain.Responses;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<Pagination<ProjectResponse>> GetAllProjects(int? page = null, int? limit = null);

        Task<Pagination<ProjectResponse>> GetProjectsOffset(int? cursor = null, int ? limit = null);
        Task<Pagination<ProjectResponse>> GetProjectsCursor(int? page = null, int ? limit = null);
        Task<int> GetUserProjectCount(Guid id);
        Task<IEnumerable<ProjectResponse>> GetUserProjects(Guid id);
        Task<IEnumerable<ProjectResponse>> GetUserDeletedProjects(Guid id);
        Task<Project> GetById(int id);
        void Add(Project project);
        Task<int> GetProjectLike(int id);
        void SoftDelete(Project project);
        void Undelete(Project project);
        void Delete(Project project);
    }
}

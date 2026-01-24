using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<Pagination<ProjectResponse>> GetAllProjects(string? filter, int? page = null, int? limit = null);

        Task<Pagination<ProjectResponse>> GetProjects(GetProjectsParameters query);
        Task<int> GetUserProjectCount(int id);
        Task<Pagination<ProjectResponse>> GetUserProjects(int id, PaginationQuery parameter);
        Task<Pagination<ProjectResponse>> GetUserDeletedProjects(int id, PaginationQuery parameter);
        Task<Project> GetById(int id);
        void Add(Project project);
        Task<int> GetProjectLike(int id);
        void SoftDelete(Project project);
        void Undelete(Project project);
        void Delete(Project project);
    }
}

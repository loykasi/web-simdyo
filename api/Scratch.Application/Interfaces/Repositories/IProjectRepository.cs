using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<Pagination<ProjectResponse>> GetAllProjects(string? filter, int? page = null, int? limit = null);
        Task<Pagination<ProjectResponse>> GetProjects(GetProjectsQuery query);
        Task<Pagination<ProjectResponse>> GetUserProjects(int id, PaginationQuery parameter);
        Task<Pagination<ProjectResponse>> GetUserDeletedProjects(int id, PaginationQuery parameter);
        Task<int> GetUserProjectCount(int id);
        
        Task<ProjectResponse> GetDetailById(int id);
        Task<Project> GetById(int id);
        void Add(Project project);
        void SoftDelete(Project project);
        void Undelete(Project project);
        void Delete(Project project);
    }
}

using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IProjectService
    {
        Task<Result<ProjectsResponse>> GetProjectsAsync();
        Task<Result<ProjectsResponse>> GetUserProjects(string userName);
        Task<Result<ProjectsResponse>> GetUserTrashAsync();
        Task<Result<ProjectResponse>> Get(string id);
        Task<Result<ProjectResponse>> Upload(UploadProjectRequest addProjectRequest);
        Task<Result> Delete(string id);
        Task<Result> Undelete(string id);
    }
}

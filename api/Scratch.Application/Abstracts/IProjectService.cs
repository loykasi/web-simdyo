using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.Application.Abstracts
{
    public interface IProjectService
    {
        Task<Result<ProjectsResponse>> GetProjectsAsync();
        Task<Result<ProjectResponse>> Get(string id);
        Task<Result<ProjectResponse>> Upload(UploadProjectRequest addProjectRequest);
        Task<Result<ProjectsResponse>> GetUserProjects(string userName);
    }
}

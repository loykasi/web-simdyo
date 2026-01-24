using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IProjectService
    {
        Task<Result<Pagination<ProjectResponse>>> GetAll(string? filter, int? page = null, int? limit = null);
        Task<Result<Pagination<ProjectResponse>>> GetProjectsAsync(GetProjectsParameters query);
        Task<Result<Pagination<ProjectResponse>>> GetUserProjects(string userName, PaginationQuery parameter);
        Task<Result<Pagination<ProjectResponse>>> GetUserTrashAsync(PaginationQuery parameter);
        Task<Result<ProjectResponse>> Get(string id);
        Task<Result<ProjectResponse>> Upload(UploadProjectRequest addProjectRequest);
        Task<Result<ProjectResponse>> Update(string publicId, UpdateProjectRequest updateProjectRequest);
        Task<Result> Delete(string id);
        Task<Result> Undelete(string id);
    }
}

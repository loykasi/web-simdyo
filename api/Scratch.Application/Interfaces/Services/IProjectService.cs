using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IProjectService
    {
        Task<Result<Pagination<ProjectResponse>>> GetAll(string? filter, int? page = null, int? limit = null);
        Task<Result<Pagination<ProjectResponse>>> GetProjectsAsync(int? lastId = null, int? page = null, int? limit = null);
        Task<Result<Pagination<ProjectResponse>>> GetUserProjects(string userName, int? page, int? limit);
        Task<Result<Pagination<ProjectResponse>>> GetUserTrashAsync(int? page = null, int? limit = null);
        Task<Result<ProjectResponse>> Get(string id);
        Task<Result<ProjectResponse>> Upload(UploadProjectRequest addProjectRequest);
        Task<Result<ProjectResponse>> Update(string publicId, UpdateProjectRequest updateProjectRequest);
        Task<Result> Delete(string id);
        Task<Result> Undelete(string id);
    }
}

using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratch.Application.Abstracts
{
    public interface IProjectService
    {
        Task<Result<GetProjectsResponse>> GetProjectsAsync();
        Task<Result<ProjectResponse>> Get(Guid id);
        Task<Result<ProjectResponse>> Upload(UploadProjectRequest addProjectRequest);
    }
}

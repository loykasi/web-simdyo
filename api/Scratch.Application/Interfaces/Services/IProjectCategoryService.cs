using Scratch.Domain.Dto;
using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Results;

namespace Scratch.Application.Interfaces.Services
{
    public interface IProjectCategoryService
    {
        Task<Result<List<ProjectCategoryDto>>> Get();
        Task<Result> Add(AddProjectCategoryRequest payload);
        Task<Result> Update(int id, UpdateProjectCategoryRequest payload);
        Task<Result> Delete(int id);
    }
}

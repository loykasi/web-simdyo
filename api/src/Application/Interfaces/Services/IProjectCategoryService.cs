using Application.Results;
using Application.Models.Responses;
using Application.Models.Requests;

namespace Application.Interfaces.Services
{
    public interface IProjectCategoryService
    {
        Task<Result<List<string>>> GetNames();
        Task<Result<List<ProjectCategoryDto>>> Get();
        Task<Result<ProjectCategoryDto>> Add(AddProjectCategoryRequest payload);
        Task<Result<ProjectCategoryDto>> Update(int id, UpdateProjectCategoryRequest payload);
        Task<Result> Delete(int id);
    }
}

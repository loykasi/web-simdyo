using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Services;
using Scratch.Domain.Dto;
using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Results;

namespace Scratch.Application.Services
{
    public class ProjectCategoryService
    (
        IUnitOfWork unitOfWork
    ): IProjectCategoryService
    {
        public async Task<Result<List<ProjectCategoryDto>>> Get()
        {
            var result = await unitOfWork.ProjectCategoryRepository.Get();
            
            return Result.Success(result);
        }

        public async Task<Result> Add(AddProjectCategoryRequest payload)
        {
            ProjectCategory category = new()
            {
                Name = payload.Name,
            };

            unitOfWork.ProjectCategoryRepository.Add(category);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> Update(int id, UpdateProjectCategoryRequest payload)
        {
            var category = await unitOfWork.ProjectCategoryRepository.GetById(id);

            if (category == null)
            {
                return Result.NotFound
                (
                    new Error("Category.NotFound", $"No category with id: {id}")
                );
            }

            category.Name = payload.Name;
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }


        public async Task<Result> Delete(int id)
        {
            await unitOfWork.ProjectCategoryRepository.Delete(id);

            return Result.Success();
        }
    }
}

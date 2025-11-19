using Scratch.Domain.Dto;
using Scratch.Domain.Entities;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IProjectCategoryRepository
    {
        Task<ProjectCategory?> GetByName(string name);
        Task<ProjectCategory?> GetById(int id);
        Task<Pagination<ProjectCategoryDto>> Get(int? page = null, int? limit = null);
        Task<List<ProjectCategoryDto>> Get();
        void Add(ProjectCategory projectCategory);
        Task Delete(int id);
    }
}

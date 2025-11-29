using Microsoft.EntityFrameworkCore;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Dto;
using Scratch.Domain.Entities;
using Scratch.Domain.Extensions;

namespace Scratch.Infrastructure.Respositories
{
    public class ProjectCategoryRepository(ApplicationDbContext dbContext) : IProjectCategoryRepository
    {
        private const int _defaultLimit = 20;

        public async Task<ProjectCategory?> GetById(int id)
        {
            return await dbContext.ProjectCategories.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ProjectCategory?> GetByName(string name)
        {
            return await dbContext.ProjectCategories.Where(c => c.Name.Equals(name)).FirstOrDefaultAsync();
        }

        public async Task<Pagination<ProjectCategoryDto>> Get(int? page = null, int? limit = null)
        {
            int pageSize = limit ?? _defaultLimit;
            int currentPage = page ?? 1;
            int offset = (currentPage - 1) * pageSize;

            var query = dbContext.ProjectCategories.OrderBy(p => p.Id);

            var items = await query.Skip(offset)
                                    .Take(pageSize)
                                    .ToListAsync();

            Pagination<ProjectCategoryDto> pagination = new()
            {
                Total = await query.CountAsync(),
                Size = items.Count,
                Items = [.. items.Select(p => p.ToProjectCategoryDto())]
            };
            return pagination;
        }

        public async Task<List<ProjectCategoryDto>> Get()
        {
            return await dbContext.ProjectCategories.OrderBy(p => p.Id)
                                                    .Select(c => c.ToProjectCategoryDto())
                                                    .ToListAsync();
        }

        public async Task<List<string>> GetNames()
        {
            return await dbContext.ProjectCategories
                .OrderBy(p => p.Id)
                .Select(c => c.Name)
                .ToListAsync();
        }

        public void Add(ProjectCategory projectCategory)
        {
            dbContext.ProjectCategories.Add(projectCategory);
        }

        public async Task Delete(int id)
        {
            await dbContext.ProjectCategories.Where(c => c.Id == id).ExecuteDeleteAsync();
        }
    }
}

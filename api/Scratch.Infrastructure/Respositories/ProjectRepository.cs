using Microsoft.EntityFrameworkCore;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Entities;
using Scratch.Domain.Extensions;
using Scratch.Domain.Responses;

namespace Scratch.Infrastructure.Respositories
{
    public class ProjectRepository(ApplicationDbContext dbContext) : IProjectRepository
    {
        private const int _defaultLimit = 20;

        private async Task<int> Count()
        {
            return await GetAvailable().Select(p => p.Id).CountAsync();
        }

        public async Task<Pagination<ProjectResponse>> GetProjectsCursor(int? cursor = null, int? limit = null)
        {
            int size = (!limit.HasValue || limit == 0) ? _defaultLimit : limit.Value;

            IQueryable<Project> query = GetAvailable().OrderByDescending(p => p.Id);
            
            if (cursor.HasValue)
            {
                query = query.Where(p => p.Id < cursor);
            }

            var items = await query.Take(size).Include(p => p.User).ToListAsync();

            Pagination<ProjectResponse> pagination = new()
            {
                Total = await Count(),
                Size = items.Count,
                LastId = items.Count == 0 ? null : items[items.Count - 1].Id,
                Items = [.. items.Select(p => p.ToProjectResponse())]
            };
            return pagination;
        }

        public async Task<Pagination<ProjectResponse>> GetProjectsOffset(int? page = null, int? limit = null)
        {
            int pageSize = limit ?? _defaultLimit;
            int currentPage = page ?? 1;
            int offset = (currentPage - 1) * pageSize;

            IQueryable<Project> query = GetAvailable().OrderByDescending(p => p.Id);

            var items = await query.Skip(offset)
                                    .Take(pageSize)
                                    .Include(p => p.User)
                                    .ToListAsync();

            Pagination<ProjectResponse> pagination = new()
            {
                Total = await query.CountAsync(),
                Size = items.Count,
                LastId = items.Count == 0 ? null : items[items.Count - 1].Id,
                Items = [.. items.Select(p => p.ToProjectResponse())]
            };
            return pagination;
        }

        public async Task<int> GetUserProjectCount(Guid id)
        {
            return await dbContext.Projects.Where(p => p.UserId == id).CountAsync();
        }

        public async Task<IEnumerable<Project>> GetUserProjects(Guid id)
        {
            return await GetAvailable().Include(p => p.User)
                                    .Where(p => p.UserId == id)
                                    .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetUserDeletedProjects(Guid id)
        {
            return await GetDeleted().Include(p => p.User)
                                    .Where(p => p.UserId == id)
                                    .ToListAsync();
        }

        public async Task<Project> GetById(int id)
        {
            return await dbContext.Projects.Include(p => p.User).FirstAsync(p => p.Id == id);
        }

        public void Add(Project project)
        {
            dbContext.Projects.Add(project);
        }

        public async Task<int> GetProjectLike(int id)
        {
            var project = await dbContext.Projects.FindAsync(id);
            return project.LikeCount;
        }

        public void SoftDelete(Project project)
        {
            project.DeletedAt = DateTime.UtcNow;
        }
        public void Undelete(Project project)
        {
            project.DeletedAt = null;
        }

        public void Delete(Project project)
        {
            dbContext.Projects.Remove(project);
        }

        private IQueryable<Project> GetAvailable()
        {
            return dbContext.Projects.Where(p => !p.DeletedAt.HasValue);
        }

        private IQueryable<Project> GetDeleted()
        {
            return dbContext.Projects.Where(p => p.DeletedAt.HasValue);
        }
    }
}

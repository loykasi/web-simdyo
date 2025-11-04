using Microsoft.EntityFrameworkCore;
using Scratch.Application.Abstracts;
using Scratch.Domain.Entities;

namespace Scratch.Infrastructure.Respositories
{
    public class ProjectRepository(ApplicationDbContext dbContext) : IProjectRepository
    {
        public async Task<IEnumerable<Project>> GetProjects()
        {
            return await dbContext.Projects.Include(p => p.User).ToListAsync();
        }

        public async Task<Project> GetById(int id)
        {
            return await dbContext.Projects.Include(p => p.User).FirstAsync(p => p.Id == id);
        }

        public void Add(Project project)
        {
            dbContext.Projects.Add(project);
        }

        public async Task<int> GetTotalProjectFromAccount(Guid id)
        {
            return await dbContext.Projects.Where(p => p.UserId == id).CountAsync();
        }

        public async Task<IEnumerable<Project>> GetUserProjects(Guid id)
        {
            return await dbContext.Projects
                                    .Include(p => p.User)
                                    .Where(p => p.UserId == id)
                                    .ToListAsync();
        }

        public async Task<int> GetProjectLike(int id)
        {
            var project = await dbContext.Projects.FindAsync(id);
            return project.LikeCount;
        }
    }
}

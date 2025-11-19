using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Entities;

namespace Scratch.Infrastructure.Respositories
{
    public class ProjectBanRepository(ApplicationDbContext dbContext) : IProjectBanRepository
    {
        public async Task<ProjectBan?> GetByProjectId(int projectId)
        {
            return dbContext.ProjectBans.Where(b => b.ProjectId == projectId && b.IsActive == true)
                                        .FirstOrDefault();
        }

        public void Add(ProjectBan projectBan)
        {
            dbContext.Add(projectBan);
        }
    }
}

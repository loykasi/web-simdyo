using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Respositories
{
    public class ProjectBanRepository(ApplicationDbContext dbContext) : IProjectBanRepository
    {
        public async Task<ProjectBan?> GetByProjectId(int projectId)
        {
            return dbContext.ProjectBans.Where(b => b.ProjectId == projectId && b.IsActive == true)
                                        .FirstOrDefault();
        }

        public async Task<bool> GetBanStatus(int projectId)
        {
            return await dbContext.ProjectBans.AnyAsync(b => b.ProjectId == projectId && b.IsActive == true);
        }

        public void Add(ProjectBan projectBan)
        {
            dbContext.Add(projectBan);
        }
    }
}

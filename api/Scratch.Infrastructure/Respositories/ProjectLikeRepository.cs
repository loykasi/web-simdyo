using Microsoft.EntityFrameworkCore;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Entities;
using System.Linq.Expressions;

namespace Scratch.Infrastructure.Respositories
{
    public class ProjectLikeRepository(ApplicationDbContext dbContext) : IProjectLikeRepository
    {
        public async Task<int> GetLikeCount(int id)
        {
            return await dbContext.ProjectLikes.Where(p => p.ProjectId == id).CountAsync();
        }

        public async Task<ProjectLike> Get(int projectId, int userId)
        {
            return await dbContext.ProjectLikes.FirstOrDefaultAsync(p => p.ProjectId == projectId && p.UserId == userId);
        }

        public void Add(ProjectLike projectLike)
        {
            dbContext.ProjectLikes.Add(projectLike);
        }

        public void Delete(ProjectLike projectLike)
        {
            dbContext.ProjectLikes.Remove(projectLike);
        }

        public async Task<bool> Exist(int projectId, int userId)
        {
            return await AnyAsync(p => p.ProjectId == projectId && p.UserId == userId);
        }

        public async Task<bool> AnyAsync(Expression<Func<ProjectLike, bool>> predicate)
        {
            return await dbContext.ProjectLikes.AnyAsync(predicate);
        }
    }
}

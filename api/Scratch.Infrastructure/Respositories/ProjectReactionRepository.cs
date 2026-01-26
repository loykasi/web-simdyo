using Microsoft.EntityFrameworkCore;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Entities;
using Scratch.Domain.Enums;
using System.Linq.Expressions;

namespace Scratch.Infrastructure.Respositories
{
    public class ProjectReactionRepository(ApplicationDbContext dbContext) : IProjectReactionRepository
    {
        public async Task<int> GetLikeCount(int id)
        {
            return await dbContext.ProjectLikes.Where(p => p.ProjectId == id).CountAsync();
        }

        public async Task<ProjectReaction?> Get(int projectId, int userId)
        {
            return await dbContext.ProjectLikes.FirstOrDefaultAsync(p => p.ProjectId == projectId && p.UserId == userId);
        }

        public void Add(ProjectReaction projectLike)
        {
            dbContext.ProjectLikes.Add(projectLike);
        }

        public void Delete(ProjectReaction projectLike)
        {
            dbContext.ProjectLikes.Remove(projectLike);
        }

        public async Task<string> GetReactionType(int projectId, int userId)
        {
            ProjectReaction? reaction = await Get(projectId, userId);

            if (reaction == null)
            {
                return string.Empty;
            }
            return reaction.Type.ToString();
        }

        public async Task<bool> AnyAsync(Expression<Func<ProjectReaction, bool>> predicate)
        {
            return await dbContext.ProjectLikes.AnyAsync(predicate);
        }
    }
}

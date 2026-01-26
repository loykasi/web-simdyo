using Scratch.Domain.Entities;
using Scratch.Domain.Enums;
using System.Linq.Expressions;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IProjectReactionRepository
    {
        Task<int> GetLikeCount(int id);
        Task<ProjectReaction?> Get(int projectId, int userId);
        Task<string> GetReactionType(int projectId, int userId);
        Task<bool> AnyAsync(Expression<Func<ProjectReaction, bool>> predicate);
        void Add(ProjectReaction projectLike);
        void Delete(ProjectReaction projectLike);
    }
}

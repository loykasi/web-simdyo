using Scratch.Domain.Entities;
using System.Linq.Expressions;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IProjectLikeRepository
    {
        Task<int> GetLikeCount(int id);
        Task<ProjectLike> Get(int projectId, Guid userId);
        Task<bool> Exist(int projectId, Guid userId);
        Task<bool> AnyAsync(Expression<Func<ProjectLike, bool>> predicate);
        void Add(ProjectLike projectLike);
        void Delete(ProjectLike projectLike);
    }
}

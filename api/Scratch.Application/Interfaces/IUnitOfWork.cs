using Scratch.Application.Interfaces.Repositories;
using Scratch.Application.Interfaces.Services;

namespace Scratch.Application.Abstracts
{
    public interface IUnitOfWork
    {
        IUserRespository UserRespository { get; }
        IProjectRepository ProjectRepository { get; }
        IProjectReactionRepository ProjectReactionRepository { get; }
        IProjectCommentRepository ProjectCommentRepository { get; }
        IUserBanRepository UserBanRepository { get; }
        IProjectReportRepository ProjectReportRepository { get; }
        IProjectBanRepository ProjectBanRepository { get; }
        IProjectCategoryRepository ProjectCategoryRepository { get; }
        IRoleRepository RoleRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

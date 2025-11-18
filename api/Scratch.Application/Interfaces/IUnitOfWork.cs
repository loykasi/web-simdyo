using Scratch.Application.Interfaces.Repositories;

namespace Scratch.Application.Abstracts
{
    public interface IUnitOfWork
    {
        IUserRespository UserRespository { get; }
        IProjectRepository ProjectRepository { get; }
        IProjectLikeRepository ProjectLikeRepository { get; }
        IProjectCommentRepository ProjectCommentRepository { get; }
        IUserBanRepository UserBanRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

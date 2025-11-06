namespace Scratch.Application.Abstracts
{
    public interface IUnitOfWork
    {
        IUserRespository UserRespository { get; }
        IProjectRepository ProjectRepository { get; }
        IProjectLikeRepository ProjectLikeRepository { get; }
        IProjectCommentRepository ProjectCommentRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

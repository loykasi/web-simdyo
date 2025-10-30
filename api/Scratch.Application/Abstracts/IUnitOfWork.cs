namespace Scratch.Application.Abstracts
{
    public interface IUnitOfWork
    {
        IUserRespository UserRespository { get; }
        IProjectRepository ProjectRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

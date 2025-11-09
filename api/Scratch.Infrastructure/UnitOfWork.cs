using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Infrastructure.Respositories;

namespace Scratch.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public IUserRespository UserRespository { get; }
        public IProjectRepository ProjectRepository { get; }
        public IProjectLikeRepository ProjectLikeRepository { get; }
        public IProjectCommentRepository ProjectCommentRepository { get; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            UserRespository = new UserRepository(dbContext);
            ProjectRepository = new ProjectRepository(dbContext);
            ProjectLikeRepository = new ProjectLikeRepository(dbContext);
            ProjectCommentRepository = new ProjectCommentRepository(dbContext);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

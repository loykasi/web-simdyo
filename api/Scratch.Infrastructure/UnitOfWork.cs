using Scratch.Application.Abstracts;
using Scratch.Infrastructure.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratch.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public IUserRespository UserRespository { get; }
        public IProjectRepository ProjectRepository { get; }
        public IProjectLikeRepository ProjectLikeRepository { get; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            UserRespository = new UserRepository(dbContext);
            ProjectRepository = new ProjectRepository(dbContext);
            ProjectLikeRepository = new ProjectLikeRepository(dbContext);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Application.Interfaces.Services;
using Scratch.Application.Services;
using Scratch.Domain.Entities;
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
        public IUserBanRepository UserBanRepository { get; }
        public IProjectReportRepository ProjectReportRepository { get; }
        public IProjectBanRepository ProjectBanRepository { get; }
        public IProjectCategoryRepository ProjectCategoryRepository { get; }
        public IRoleRepository RoleRepository { get; }

        public UnitOfWork(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            UserRespository = new UserRepository(dbContext, userManager);
            ProjectRepository = new ProjectRepository(dbContext);
            ProjectLikeRepository = new ProjectLikeRepository(dbContext);
            ProjectCommentRepository = new ProjectCommentRepository(dbContext);
            UserBanRepository = new UserBanRepository(dbContext);
            ProjectReportRepository = new ProjectReportRepository(dbContext);
            ProjectBanRepository = new ProjectBanRepository(dbContext);
            ProjectCategoryRepository = new ProjectCategoryRepository(dbContext);
            RoleRepository = new RoleRepository(dbContext);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

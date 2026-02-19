using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Respositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure
{
    public class UnitOfWork(
        ApplicationDbContext dbContext,
        UserManager<User> userManager,
        RoleManager<Role> roleManager
    ) : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public IUserRespository UserRespository { get; } = new UserRepository(dbContext, userManager);
        public IProjectRepository ProjectRepository { get; } = new ProjectRepository(dbContext);
        public IProjectReactionRepository ProjectReactionRepository { get; } = new ProjectReactionRepository(dbContext);
        public IProjectCommentRepository ProjectCommentRepository { get; } = new ProjectCommentRepository(dbContext);
        public IUserBanRepository UserBanRepository { get; } = new UserBanRepository(dbContext);
        public IProjectReportRepository ProjectReportRepository { get; } = new ProjectReportRepository(dbContext);
        public IProjectBanRepository ProjectBanRepository { get; } = new ProjectBanRepository(dbContext);
        public IProjectCategoryRepository ProjectCategoryRepository { get; } = new ProjectCategoryRepository(dbContext);
        public IRoleRepository RoleRepository { get; } = new RoleRepository(dbContext, roleManager);
        public IUserDailyUploadStatsRepository UserDailyUploadStatsRepository { get; } = new UserDailyUploadStatsRepository(dbContext);
        public INotificationRepository NotificationRepository { get; } = new NotificationRepository(dbContext);

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

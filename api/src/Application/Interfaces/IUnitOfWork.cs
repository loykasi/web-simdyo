using Application.Interfaces.Repositories;

namespace Application.Interfaces
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
        IUserDailyUploadStatsRepository UserDailyUploadStatsRepository { get; }
        INotificationRepository NotificationRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

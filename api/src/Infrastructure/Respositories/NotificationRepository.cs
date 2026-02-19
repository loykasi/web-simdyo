using Application.Interfaces.Repositories;
using Application.Models.Responses.Notification;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Respositories
{
    internal class NotificationRepository(ApplicationDbContext dbContext) : INotificationRepository
    {
        public async Task<int> GetUnreadCount(int userId)
        {
            return await dbContext.Notifications
                .Where(x => x.UserId == userId && !x.ReadAt.HasValue)
                .CountAsync();
        }

        public void Add(Notification notification)
        {
            dbContext.Notifications.Add(notification);
        }

        public Task<List<NotificationDto>> GetByUserId(int id)
        {
            return dbContext.Notifications
                .Where(x => x.UserId == id)
                .Select(x => new NotificationDto
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    Type = x.Type,
                    ActorUsername = x.Actor.Username,
                    ProjectId = x.Project != null ? x.Project.PublicId : null,
                    ProjectName = x.Project != null ? x.Project.Name : null,
                })
                .ToListAsync();
        }
    }
}

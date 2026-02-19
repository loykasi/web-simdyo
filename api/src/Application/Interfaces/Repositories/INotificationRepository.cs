using Application.Models.Responses.Notification;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<int> GetUnreadCount(int userId);
        Task<List<NotificationDto>> GetByUserId(int id);
        void Add(Notification notification);
    }
}

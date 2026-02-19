using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Services
{
    public interface INotificationService
    {
        void CreateCommentNotification(User actor, User user, ProjectComment comment);
        Task CreateAsync(User actor, User user, NotificationTypes type, string Message);
    }
}

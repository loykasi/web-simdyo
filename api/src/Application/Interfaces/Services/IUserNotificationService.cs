using Application.Models.Responses.Notification;
using Application.Results;

namespace Application.Interfaces.Services
{
    public interface IUserNotificationService
    {
        Task<Result<int>> GetUnreadCount();
        Task<Result<List<NotificationDto>>> Get();
        Task<Result> Create();
    }
}

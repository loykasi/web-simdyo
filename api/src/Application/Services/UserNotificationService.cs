using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Models.Responses.Notification;
using Application.Results;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserNotificationService
    (
        ICurrentUserService currentUserService,
        INotificationService notificationService,
        IUnitOfWork unitOfWork,
        UserManager<User> userManager
    ) : IUserNotificationService
    {
        public async Task<Result<int>> GetUnreadCount()
        {
            var user = await currentUserService.GetUserAsync();
            if (user == null)
            {
                return Result.UnAuthorized<int>(new Error("Unauthorized", "Unauthorized"));
            }

            int count = await unitOfWork.NotificationRepository.GetUnreadCount(user.Id);
            return Result.Success(count);
        }

        public async Task<Result> Create()
        {
            var user1 = await userManager.FindByIdAsync("1");

            ProjectComment comment = new()
            {
                Content = "Hello World!"
            };

            notificationService.CreateCommentNotification(user1!, user1!, comment);
            return Result.Success();
        }

        public async Task<Result<List<NotificationDto>>> Get()
        {
            var user = await currentUserService.GetUserAsync();
            if (user == null)
            {
                return Result.UnAuthorized<List<NotificationDto>>
                (
                    new Error("Unauthorized", "Unauthorized")
                );
            }

            var notifications = await unitOfWork.NotificationRepository.GetByUserId(user.Id);
            return Result.Success(notifications);
        }
    }
}

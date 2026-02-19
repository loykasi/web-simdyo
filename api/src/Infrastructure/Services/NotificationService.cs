using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;
using Hangfire;

namespace Infrastructure.Services
{
    public class NotificationService(IUnitOfWork unitOfWork) : INotificationService
    {
        public async Task CreateAsync(User actor, User user, NotificationTypes type, string Message)
        {
            Notification notification = new()
            {
                User = user,
                Actor = actor,
                Type = type,
                Message = Message
            };
            unitOfWork.NotificationRepository.Add(notification);
            await unitOfWork.SaveChangesAsync();

            throw new NotImplementedException();
        }

        public void CreateCommentNotification(User actor, User user, ProjectComment comment)
        {
            BackgroundJob.Enqueue(() => CreateNotificationJob(
                actor.Id,
                user.Id,
                "{actor} commented on your project {project}: " + comment.Content,
                1
            ));
        }

        public async Task CreateNotificationJob(int actorId, int userId, string message, int projectId)
        {
            Notification notification = new()
            {
                UserId = userId,
                ActorId = actorId,
                Type = NotificationTypes.ProjectCommented,
                Message = message,
                ProjectId = 1
            };
            unitOfWork.NotificationRepository.Add(notification);
            await unitOfWork.SaveChangesAsync();
        }
    }
}

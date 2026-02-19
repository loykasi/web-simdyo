using Domain.Enums;

namespace Application.Models.Responses.Notification
{
    public class NotificationDto
    {
        public required int Id { get; set; }
        public required NotificationTypes Type { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string ActorUsername { get; set; }
        
        public string? ProjectId { get; set; }
        public string? ProjectName { get; set; }
    }
}

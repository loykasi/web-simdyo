using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Notification : ITrackable
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty(nameof(User.Notifications))]
        public User User { get; set; }

        public int ActorId { get; set; }
        [ForeignKey("ActorId")]
        [InverseProperty(nameof(User.TriggerNotifications))]
        public User Actor { get; set; }

        public required NotificationTypes Type { get; set; }
        public required string Message { get; set; }
        
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public DateTime? ReadAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

using Domain.Enums;

namespace Domain.Entities
{
    public class ProjectReaction : ITrackable
    {
        public int Id { get; set; }
        public ReactionTypes Type { get; set; }
        
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

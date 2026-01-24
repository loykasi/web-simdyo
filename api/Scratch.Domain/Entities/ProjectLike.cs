namespace Scratch.Domain.Entities
{
    public class ProjectLike : ITrackable
    {
        public int Id { get; set; }
        
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

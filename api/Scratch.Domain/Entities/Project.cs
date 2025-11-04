namespace Scratch.Domain.Entities
{
    public class Project : ITrackable
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? PublicId { get; set; }
        public string? FileLink { get; set; }
        public string? ThumbnailLink { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public int LikeCount { get; set; }
        public ICollection<ProjectLike> ProjectLikes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
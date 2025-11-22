namespace Scratch.Domain.Entities
{
    public class Project : ITrackable, ISoftDeletable
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        //public required string Category { get; set; }
        public string PublicId { get; set; } = "";
        public string FileLink { get; set; } = "";
        public string ThumbnailLink { get; set; } = "";

        public int CategoryId { get; set; } = 1;
        public required ProjectCategory Category { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public int LikeCount { get; set; }
        public ICollection<ProjectLike> ProjectLikes { get; set; }

        public ICollection<ProjectBan> ProjectBans { get; set; }

        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
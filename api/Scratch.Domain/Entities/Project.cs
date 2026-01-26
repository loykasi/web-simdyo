namespace Scratch.Domain.Entities
{
    public class Project : ITrackable, ISoftDeletable
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ShortDescription { get; set; }
        public required string Description { get; set; }
        public string PublicId { get; set; } = "";
        public string FileLink { get; set; } = "";
        public string ThumbnailLink { get; set; } = "";

        public int? CategoryId { get; set; }
        public ProjectCategory? Category { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<ProjectReaction> ProjectReactions { get; set; }
        public ICollection<ProjectBan> ProjectBans { get; set; }

        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
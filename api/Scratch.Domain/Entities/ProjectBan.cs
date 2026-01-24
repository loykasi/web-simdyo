namespace Scratch.Domain.Entities
{
    public class ProjectBan : ITrackable
    {
        public int Id { get; set; }
        public required string Reason { get; set; }
        public string? Description { get; set; }

        public int ProjectId { get; set; }
        public required Project Project { get; set; }

        public int ByUserId { get; set; }
        public required User ByUser { get; set; }

        public bool IsActive { get; set; } = true;
        public int? RevokedByUserId { get; set; }
        public User? RevokedByUser { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

namespace Scratch.Domain.Entities
{
    public class ProjectReport : ITrackable
    {
        public int Id { get; set; }
        public required string Reason { get; set; }
        public string Description { get; set; } = string.Empty;

        public int ProjectId { get; set; }
        public required Project Project { get; set; }

        public Guid ByUserId { get; set; }
        public required User ByUser { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

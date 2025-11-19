namespace Scratch.Domain.Entities
{
    public class ProjectCategory : ITrackable
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

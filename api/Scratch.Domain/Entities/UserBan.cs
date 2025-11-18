namespace Scratch.Domain.Entities
{
    public class UserBan : ITrackable
    {
        public int Id { get; set; }
        public required string Reason { get; set; }
        public string? Description { get; set; }

        public Guid UserId { get; set; }
        public required User User { get; set; }

        public Guid ByUserId { get; set; }
        public required User ByUser { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

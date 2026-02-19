namespace Domain.Entities
{
    public class UserBan : ITrackable
    {
        public int Id { get; set; }
        public required string Reason { get; set; }
        public string? Description { get; set; }

        public int UserId { get; set; }
        public required User User { get; set; }

        public int ByUserId { get; set; }
        public required User ByUser { get; set; }

        public bool IsActive { get; set; }
        public int? RevokedByUserId { get; set; }
        public User? RevokedByUser { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

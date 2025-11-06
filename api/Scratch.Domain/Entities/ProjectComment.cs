using System.ComponentModel.DataAnnotations.Schema;

namespace Scratch.Domain.Entities
{
    public class ProjectComment : ITrackable
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int? ParentId { get; set; } = null;

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty(nameof(User.Comments))]
        public User User { get; set; } = null!;

        public Guid? RepliedUserId { get; set; } = null;
        [ForeignKey("RepliedUserId")]
        [InverseProperty(nameof(User.RepliedComments))]
        public User? RepliedUser { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

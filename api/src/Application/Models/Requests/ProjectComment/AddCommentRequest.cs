using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests.ProjectComment
{
    public class AddCommentRequest
    {
        [StringLength(1000)]
        public required string Content { get; set; }
        public int? ParentId { get; set; }
        public string? RepliedUsername { get; set; }
    }
}

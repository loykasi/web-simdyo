namespace Scratch.Domain.Requests
{
    public class AddCommentRequest
    {
        public required string Content { get; set; }
        public int? ParentId { get; set; }
        public string? RepliedUsername { get; set; }
    }
}

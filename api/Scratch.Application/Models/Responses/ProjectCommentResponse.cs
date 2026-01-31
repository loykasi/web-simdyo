namespace Scratch.Application.Models.Responses
{
    public record ProjectCommentResponse
    (
        int Id,
        string Content,
        int? ParentId,
        string Username,
        string RepliedUsername,
        string CreatedAt,
        int TotalReplies
    );
}

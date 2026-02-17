namespace Scratch.Application.Models.Responses
{
    public record ProjectResponse
    (
        string PublicId,
        string Title,
        string Description,
        string? Category,
        string ProjectLink,
        string ThumbnailLink,
        string Username,
        int LikeCount,
        int OkayCount,
        bool IsBanned,
        string CreatedAt,
        string? DeletedAt
    );
}

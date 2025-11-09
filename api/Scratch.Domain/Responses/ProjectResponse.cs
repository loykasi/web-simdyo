using Microsoft.AspNetCore.Http;

namespace Scratch.Domain.Responses
{
    public record ProjectResponse
    (
        string? PublicId,
        string? Title,
        string? Description,
        string? Category,
        string? ProjectLink,
        string? ThumbnailLink,
        string? Username,
        int likeCount,
        string? CreatedAt,
        string? DeletedAt
    );
}

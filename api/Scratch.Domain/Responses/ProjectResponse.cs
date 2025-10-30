using Microsoft.AspNetCore.Http;

namespace Scratch.Domain.Responses
{
    public record ProjectResponse
    (
        Guid? Id,
        string? Title,
        string? Description,
        string? Category,
        string? ProjectLink,
        string? ThumbnailLink
    );
}

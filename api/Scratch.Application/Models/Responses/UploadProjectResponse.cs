namespace Scratch.Application.Models.Responses
{
    public record UploadProjectResponse
    (
        string PublicId,
        string ProjectPresignedUrl,
        string ThumbnaiPresignedUrl
    );
}

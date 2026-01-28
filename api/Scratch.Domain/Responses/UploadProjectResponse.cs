namespace Scratch.Domain.Responses
{
    public record UploadProjectResponse
    (
        string PublicId,
        string ProjectPresignedUrl,
        string ThumbnaiPresignedUrl
    );
}

namespace Scratch.Domain.Responses
{
    public record ProjectUploadResponse
    (
        string PublicId,
        string ProjectPresignedUrl,
        string ThumbnaiPresignedUrl
    );
}

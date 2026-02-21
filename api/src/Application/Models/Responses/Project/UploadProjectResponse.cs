namespace Application.Models.Responses.Project
{
    public record UploadProjectResponse
    (
        string PublicId,
        string ProjectPresignedUrl,
        string ThumbnaiPresignedUrl
    );
}

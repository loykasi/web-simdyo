using Microsoft.AspNetCore.Http;

namespace Scratch.Domain.Requests
{
    public record UploadProjectRequest
    {
        public required string Title { get; set; }
        public required string ShortDescription { get; set; }
        public required string Description { get; set; }
        public string? Category { get; set; }
        public required long ProjectLength { get; set; }
        public required long ThumbnailLength { get; set; }
        //public required IFormFile ProjectFile { get; set; }
        //public required IFormFile ThumbnailFile { get; set; }
    }
}

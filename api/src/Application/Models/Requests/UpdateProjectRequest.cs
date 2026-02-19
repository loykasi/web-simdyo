using Microsoft.AspNetCore.Http;

namespace Application.Models.Requests
{
    public record UpdateProjectRequest
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public string? Category { get; set; }
        public long? ProjectLength { get; set; }
        public long? ThumbnailLength { get; set; }
        //public IFormFile? ProjectFile { get; set; }
        //public IFormFile? ThumbnailFile { get; set; }
    }
}

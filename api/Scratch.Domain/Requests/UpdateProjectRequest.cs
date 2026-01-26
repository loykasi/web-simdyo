using Microsoft.AspNetCore.Http;

namespace Scratch.Domain.Requests
{
    public record UpdateProjectRequest
    {
        public required string Title { get; set; }
        public required string ShortDescription { get; set; }
        public required string Description { get; set; }
        public string? Category { get; set; }
        public IFormFile? ProjectFile { get; set; }
        public IFormFile? ThumbnailFile { get; set; }
    }
}

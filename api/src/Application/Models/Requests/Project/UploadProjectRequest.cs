using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests.Project
{
    public record UploadProjectRequest
    {
        [StringLength(60)]
        public required string Title { get; set; }
        [StringLength(3000)]
        public required string Description { get; set; }
        public string? Category { get; set; }
        public required long ProjectLength { get; set; }
        public required long ThumbnailLength { get; set; }
    }
}

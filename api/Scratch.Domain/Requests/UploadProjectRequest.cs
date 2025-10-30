using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratch.Domain.Requests
{
    public record UploadProjectRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public required IFormFile ProjectFile { get; set; }
        public required IFormFile ThumbnailFile { get; set; }
    }
}

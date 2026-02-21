using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests.Project
{
    public record ReportProjectRequest
    {
        public required string Reason { get; set; }
        [StringLength(3000)]
        public string Description { get; set; } = string.Empty;
    }
}

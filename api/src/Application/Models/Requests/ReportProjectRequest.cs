namespace Application.Models.Requests
{
    public record ReportProjectRequest
    {
        public required string Reason { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

namespace Application.Models.Requests.Project
{
    public record BanProjectRequest
    {
        public required string Reason { get; set; }
        public string? Description { get; set; }
    }
}

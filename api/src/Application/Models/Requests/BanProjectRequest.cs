namespace Application.Models.Requests
{
    public record BanProjectRequest
    {
        public required string Reason { get; set; }
        public string? Description { get; set; }
    }
}

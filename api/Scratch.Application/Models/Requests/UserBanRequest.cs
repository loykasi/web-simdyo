namespace Scratch.Application.Models.Requests
{
    public record UserBanRequest
    {
        public required string Reason { get; set; }
        public string? Description { get; set; }
    }
}

namespace Application.Models.Requests
{
    public record LoginOtpRequest
    {
        public required string Email { get; set; }
        public required string Code { get; set; }
    }
}

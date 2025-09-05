namespace Scratch.Domain.Requests
{
    public record ForgotPasswordRequest
    {
        public required string Email { get; set; }
    }
}

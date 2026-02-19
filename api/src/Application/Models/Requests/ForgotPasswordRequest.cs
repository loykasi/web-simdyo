namespace Application.Models.Requests
{
    public record ForgotPasswordRequest
    {
        public required string Email { get; set; }
    }
}

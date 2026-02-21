namespace Application.Models.Requests.Auth
{
    public record ForgotPasswordRequest
    {
        public required string Email { get; set; }
    }
}

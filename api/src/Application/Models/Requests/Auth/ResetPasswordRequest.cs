namespace Application.Models.Requests.Auth
{
    public record ResetPasswordRequest
    {
        public required string Token { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}

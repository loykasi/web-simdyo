namespace Application.Models.Requests
{
    public record ResetPasswordRequest
    {
        public required string Token { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}

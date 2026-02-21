namespace Application.Models.Requests.Auth
{
    public record ChangePasswordRequest
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}

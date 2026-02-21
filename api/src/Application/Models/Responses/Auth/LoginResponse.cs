namespace Application.Models.Responses.Auth
{
    public record LoginResponse
    (
        string Username,
        string Email,
        string ExpiresAt,
        bool IsUseOTP,
        string[] Permissions
    );
}

namespace Scratch.Domain.Responses
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

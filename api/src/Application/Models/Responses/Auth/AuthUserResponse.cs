namespace Application.Models.Responses.Auth
{
    public record AuthUserResponse
    (
        string Username,
        string Email,
        bool IsBanned,
        string[] Permissions
    );
}

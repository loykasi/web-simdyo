namespace Application.Models.Responses
{
    public record AuthUserResponse
    (
        string Username,
        string Email,
        bool IsBanned,
        string[] Permissions
    );
}

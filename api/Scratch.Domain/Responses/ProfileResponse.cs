namespace Scratch.Domain.Responses
{
    public record ProfileResponse
    (
        string Username,
        string Email,
        bool IsBanned
    );
}

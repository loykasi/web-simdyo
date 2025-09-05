namespace Scratch.Domain.Requests
{
    public record ConfirmEmailRequest(string Token, string Email);
}

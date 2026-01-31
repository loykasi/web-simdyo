namespace Scratch.Application.Models.Requests
{
    public record ConfirmEmailRequest(string Token, string Email);
}

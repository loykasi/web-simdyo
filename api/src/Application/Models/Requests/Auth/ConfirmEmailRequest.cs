namespace Application.Models.Requests.Auth
{
    public record ConfirmEmailRequest(string Token, string Email);
}

using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests.Auth
{
    public record LoginRequest
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests.Auth
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(40)]
        public required string Username { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
    }
}

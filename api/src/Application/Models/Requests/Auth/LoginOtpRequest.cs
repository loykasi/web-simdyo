using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests.Auth
{
    public record LoginOtpRequest
    {
        [EmailAddress]
        public required string Email { get; set; }
        [StringLength(6, MinimumLength = 6)]
        public required string Code { get; set; }
    }
}

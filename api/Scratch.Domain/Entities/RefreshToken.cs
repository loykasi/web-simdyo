using Microsoft.AspNetCore.Identity;

namespace Scratch.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public required string Token { get; set; }
        public required DateTime RefreshTokenExpriresAtUTC { get; set; }

        public Guid UserId { get; set; }
        public required User User { get; set; }
    }
}

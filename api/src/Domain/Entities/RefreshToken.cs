using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public required string Token { get; set; }
        public required DateTime RefreshTokenExpriresAtUTC { get; set; }

        public int UserId { get; set; }
        public required User User { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace Scratch.Domain.Entities
{
    public class User: IdentityUser<Guid>, ITrackable
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpriresAtUTC { get; set; }

        public ICollection<Project> Projects { get; set; }
        public ICollection<ProjectLike> ProjectLikes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static User Create(string email, string userName)
        {
            return new User
            {
                Email = email,
                UserName = userName
            };
        }

        public override string ToString()
        {
            return UserName;
        }
    }
}

using Microsoft.AspNetCore.Identity;

namespace Scratch.Domain.Entities
{
    public class User: IdentityUser<int>, ITrackable
    {
        public string Username { get => UserName!; set => UserName = value; }
        public string LastUsedToken { get; set; } = string.Empty;

        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
        public ICollection<Project> Projects { get; set; } = [];
        public ICollection<ProjectReaction> ProjectLikes { get; set; } = [];
        public ICollection<ProjectComment> Comments { get; set; } = [];
        public ICollection<ProjectComment> RepliedComments { get; set; } = [];

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static User Create(string email, string userName)
        {
            return new User
            {
                Email = email,
                Username = userName
            };
        }

        public override string ToString()
        {
            return Username;
        }
    }
}

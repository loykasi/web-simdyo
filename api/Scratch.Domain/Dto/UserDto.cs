namespace Scratch.Domain.DTO
{
    public record UserDto
    {
        public required int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string[] Roles { get; set; }
        public required bool IsBanned { get; set; }
        public required string CreatedAt { get; set; }
    }
}

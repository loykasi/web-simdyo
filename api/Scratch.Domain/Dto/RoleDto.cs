namespace Scratch.Domain.Dto
{
    public record RoleDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string[] Permissions { get; set; }
    }
}

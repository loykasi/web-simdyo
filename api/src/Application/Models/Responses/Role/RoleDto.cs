namespace Application.Models.Responses.Role
{
    public record RoleDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string[] Permissions { get; set; }
    }
}

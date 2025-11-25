namespace Scratch.Domain.Requests
{
    public record SetUserRoleRequest
    {
        public required string[] Roles { get; set; }
    }
}

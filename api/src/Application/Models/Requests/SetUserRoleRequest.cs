namespace Application.Models.Requests
{
    public record SetUserRoleRequest
    {
        public required string[] Roles { get; set; }
    }
}

namespace Application.Models.Requests.Role
{
    public record SetUserRoleRequest
    {
        public required string[] Roles { get; set; }
    }
}

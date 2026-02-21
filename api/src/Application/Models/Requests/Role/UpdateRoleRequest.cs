namespace Application.Models.Requests.Role
{
    public class UpdateRoleRequest
    {
        public required string Name { get; set; }
        public string[]? Enables { get; set; }
        public string[]? Disables { get; set; }
    }
}

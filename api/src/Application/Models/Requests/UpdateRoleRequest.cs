namespace Application.Models.Requests
{
    public class UpdateRoleRequest
    {
        public required string Name { get; set; }
        public string[]? Enables { get; set; }
        public string[]? Disables { get; set; }
    }
}

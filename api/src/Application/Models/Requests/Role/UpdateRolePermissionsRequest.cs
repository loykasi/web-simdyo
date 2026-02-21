namespace Application.Models.Requests.Role
{
    public class UpdateRolePermissionsRequest
    {
        public string[]? Enables { get; set; }
        public string[]? Disables { get; set; }
    }
}

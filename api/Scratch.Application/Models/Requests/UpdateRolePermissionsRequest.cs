namespace Scratch.Application.Models.Requests
{
    public class UpdateRolePermissionsRequest
    {
        public string[]? Enables { get; set; }
        public string[]? Disables { get; set; }
    }
}

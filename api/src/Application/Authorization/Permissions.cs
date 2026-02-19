namespace Application.Authorization
{
    public class Permissions
    {
        public const string DashboardAccess = "dashboard_access";
        public const string ManageUsers = "manage_users";
        public const string ManageProjects = "manage_projects";
        public const string ManageProjectReport = "manage_project_reports";
        public const string ManageCategories = "manage_categories";
        public const string ManageRoles = "manage_roles";

        public static IEnumerable<string> All => [
            DashboardAccess,
            ManageUsers,
            ManageProjects,
            ManageProjectReport,
            ManageCategories,
            ManageRoles
        ];
    }
}

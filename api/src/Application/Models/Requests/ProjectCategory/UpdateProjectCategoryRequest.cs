namespace Application.Models.Requests.ProjectCategory
{
    public record UpdateProjectCategoryRequest
    {
        public required string Name { get; set; }
    }
}

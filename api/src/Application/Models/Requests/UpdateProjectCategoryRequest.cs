namespace Application.Models.Requests
{
    public record UpdateProjectCategoryRequest
    {
        public required string Name { get; set; }
    }
}

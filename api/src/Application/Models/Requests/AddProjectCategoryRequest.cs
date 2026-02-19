namespace Application.Models.Requests
{
    public record AddProjectCategoryRequest
    {
        public required string Name { get; set; }
    }
}

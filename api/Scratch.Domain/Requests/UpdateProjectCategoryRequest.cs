namespace Scratch.Domain.Requests
{
    public record UpdateProjectCategoryRequest
    {
        public required string Name { get; set; }
    }
}

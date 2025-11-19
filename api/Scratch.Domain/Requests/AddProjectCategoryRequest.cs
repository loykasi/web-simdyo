namespace Scratch.Domain.Requests
{
    public record AddProjectCategoryRequest
    {
        public required string Name { get; set; }
    }
}

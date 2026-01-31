namespace Scratch.Application.Models.Responses
{
    public record ProjectCategoryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string CreatedAt { get; set; }
        public required string UpdatedAt { get; set; }
    }
}

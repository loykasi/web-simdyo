namespace Application.Models.Responses
{
    public record ProjectReportDto
    {
        public int Id { get; set; }
        public required string Reason { get; set; }
        public required string Description { get; set; }
        public required string ProjectPublicId { get; set; }
        public required string Username { get; set; }
        public required string CreatedAt { get; set; }
    }
}

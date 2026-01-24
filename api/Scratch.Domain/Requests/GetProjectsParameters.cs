namespace Scratch.Domain.Requests
{
    public class GetProjectsParameters : PaginationQuery
    {
        public string? Search {  get; set; }
        public string? Category { get; set; }

        public string SearchTerm => (Search ?? string.Empty).ToLower();
    }
}

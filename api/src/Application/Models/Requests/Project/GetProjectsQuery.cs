using System.Text.Json.Serialization;

namespace Application.Models.Requests.Project
{
    public class GetProjectsQuery : PaginationQuery
    {
        public string? Search {  get; set; }
        public string? Category { get; set; }

        [JsonIgnore]
        public string SearchTerm => (Search ?? string.Empty).ToLower();
    }
}

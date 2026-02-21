using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests.ProjectCategory
{
    public record AddProjectCategoryRequest
    {
        [StringLength(50)]
        public required string Name { get; set; }
    }
}

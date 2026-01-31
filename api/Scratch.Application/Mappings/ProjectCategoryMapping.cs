using Scratch.Application.Models.Responses;
using Scratch.Domain.Entities;

namespace Scratch.Application.Mappings
{
    public static class ProjectCategoryMapping
    {
        public static ProjectCategoryDto ToProjectCategoryDto(this ProjectCategory category)
        {
            return new ProjectCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                CreatedAt = category.CreatedAt.ToString("o"),
                UpdatedAt = category.UpdatedAt.ToString("o"),
            };
        }
    }
}

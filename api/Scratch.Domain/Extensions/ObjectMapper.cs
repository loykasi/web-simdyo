using Scratch.Domain.Dto;
using Scratch.Domain.Entities;
using Scratch.Domain.Responses;

namespace Scratch.Domain.Extensions
{
    public static class ObjectMapper
    {
        public static ProjectResponse ToProjectResponse(this Project project)
        {
            return new ProjectResponse
                (
                    project.PublicId,
                    project.Name,
                    project.ShortDescription,
                    project.Description,
                    project.Category != null ? project.Category.Name : null,
                    project.FileLink,
                    project.ThumbnailLink,
                    project.User.UserName!,
                    0,
                    0,
                    false,
                    project.CreatedAt.ToString("o"),
                    project.DeletedAt.HasValue ? project.DeletedAt.Value.ToString("o") : null
                );
        }

        public static ProjectCategoryDto ToProjectCategoryDto(this ProjectCategory category)
        {
            return new ProjectCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                CreatedAt = category.CreatedAt.ToString("o"),
                UpdatedAt = category.UpdatedAt.ToString("o")
            };
        }
    }
}

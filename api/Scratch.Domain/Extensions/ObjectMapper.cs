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
                    project.Description,
                    project.Category,
                    project.FileLink,
                    project.ThumbnailLink,
                    project.User.UserName,
                    project.LikeCount,
                    project.CreatedAt.ToString("o"),
                    project.DeletedAt.HasValue ? project.DeletedAt.Value.ToString("o") : null
                );
        }
    }
}

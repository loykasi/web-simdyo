using Scratch.Domain.Entities;

namespace Scratch.Domain.Responses
{
    public record GetProjectsResponse(List<Project> Projects);
}

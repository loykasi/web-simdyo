using Scratch.Domain.Dto;
using Scratch.Domain.Entities;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IProjectReportRepository
    {
        Task<Pagination<ProjectReportDto>> Get(string? filter, int? page = null, int? limit = null);
        void Add(ProjectReport report);
    }
}

using Application.Models.Responses;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IProjectReportRepository
    {
        Task<Pagination<ProjectReportDto>> Get(string? filter, int? page = null, int? limit = null);
        void Add(ProjectReport report);
    }
}

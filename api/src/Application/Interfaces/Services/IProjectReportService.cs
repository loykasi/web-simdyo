using Application.Results;
using Domain.Entities;
using Application.Models.Requests.Project;
using Application.Models.Responses.Project;

namespace Application.Interfaces.Services
{
    public interface IProjectReportService
    {
        Task<Result<Pagination<ProjectReportDto>>> Get(string? filter, int? page = null, int? limit = null);
        Task<Result> AddReport(string publicId, ReportProjectRequest payload);
    }
}

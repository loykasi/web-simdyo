using Application.Models.Responses;
using Application.Results;
using Application.Models.Requests;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IProjectReportService
    {
        Task<Result<Pagination<ProjectReportDto>>> Get(string? filter, int? page = null, int? limit = null);
        Task<Result> AddReport(string publicId, ReportProjectRequest payload);
    }
}

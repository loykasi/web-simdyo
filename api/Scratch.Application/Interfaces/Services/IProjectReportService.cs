using Scratch.Domain.Dto;
using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Results;

namespace Scratch.Application.Interfaces.Services
{
    public interface IProjectReportService
    {
        Task<Result<Pagination<ProjectReportDto>>> Get(string? filter, int? page = null, int? limit = null);
        Task<Result> AddReport(string publicId, ReportProjectRequest payload);
    }
}

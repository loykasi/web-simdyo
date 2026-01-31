using Scratch.Domain.Dto;
using Scratch.Domain.Entities;
using Scratch.Application.Models.Requests;
using Scratch.Application.Results;

namespace Scratch.Application.Interfaces.Services
{
    public interface IProjectReportService
    {
        Task<Result<Pagination<ProjectReportDto>>> Get(string? filter, int? page = null, int? limit = null);
        Task<Result> AddReport(string publicId, ReportProjectRequest payload);
    }
}

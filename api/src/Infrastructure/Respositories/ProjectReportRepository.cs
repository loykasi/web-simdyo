using Application.Interfaces.Repositories;
using Application.Models.Responses.Project;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Respositories
{
    public class ProjectReportRepository(ApplicationDbContext dbContext) : IProjectReportRepository
    {
        private const int _defaultLimit = 20;

        public async Task<Pagination<ProjectReportDto>> Get(
            string? filter, int? page = null, int? limit = null)
        {
            int pageSize = limit ?? _defaultLimit;
            int currentPage = page ?? 1;
            int offset = (currentPage - 1) * pageSize;

            string searchTerm = (filter ?? string.Empty).ToLower();

            var query = dbContext.ProjectReports
                .Where(r =>
                    r.Reason.ToLower().Contains(searchTerm) ||
                    r.Project.PublicId.ToLower().Contains(searchTerm) ||
                    r.ByUser.UserName!.ToLower().Contains(searchTerm)
                )
                .OrderByDescending(r => r.CreatedAt)
                .AsNoTracking();

            var items = await query.Skip(offset)
                                    .Take(pageSize)
                                    .Include(r => r.ByUser)
                                    .Include(r => r.Project)
                                    .Select(r => new ProjectReportDto
                                    {
                                        Id = r.Id,
                                        Reason = r.Reason,
                                        Description = r.Description,
                                        ProjectPublicId = r.Project.PublicId,
                                        Username = r.ByUser.UserName,
                                        CreatedAt = r.CreatedAt.ToString("o")
                                    })
                                    .ToListAsync();

            Pagination<ProjectReportDto> pagination = new()
            {
                Total = await query.CountAsync(),
                Size = items.Count,
                Items = items
            };

            return pagination;
        }

        public void Add(ProjectReport report)
        {
            dbContext.ProjectReports.Add(report);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using System;
using System.Linq;

namespace Scratch.Infrastructure.Respositories
{
    public class ProjectRepository(ApplicationDbContext dbContext) : IProjectRepository
    {
        private const int _defaultLimit = 20;

        public async Task<Pagination<ProjectResponse>> GetAllProjects(string? filter, int? page = null, int? limit = null)
        {
            int pageSize = limit ?? _defaultLimit;
            int currentPage = page ?? 1;
            int offset = (currentPage - 1) * pageSize;

            string searchTerm = (filter ?? string.Empty).ToLower();

            IQueryable<Project> query = dbContext.Projects
                .Where(p =>
                    p.Name.ToLower().Contains(searchTerm)
                );

            int total = await query.CountAsync();

            var items = await query.OrderByDescending(p => p.Id)
                .Skip(offset)
                .Take(pageSize)
                .Include(p => p.User)
                .Include(p => p.Category)
                .Select(p => new
                {
                    p.Id,
                    p.PublicId,
                    p.Name,
                    p.Description,
                    Category = p.Category.Name,
                    p.FileLink,
                    p.ThumbnailLink,
                    Username = p.User.UserName,
                    p.LikeCount,
                    IsBanned = dbContext.ProjectBans.Any(b => b.ProjectId == p.Id && b.IsActive == true),
                    p.CreatedAt,
                    p.DeletedAt
                })
                .ToListAsync();

            var responseItems = items.Select(p => new ProjectResponse(
                                            p.PublicId,
                                            p.Name,
                                            p.Description,
                                            p.Category,
                                            p.FileLink,
                                            p.ThumbnailLink,
                                            p.Username,
                                            p.LikeCount,
                                            p.IsBanned,
                                            p.CreatedAt.ToString("o"),
                                            p.DeletedAt?.ToString("o")
                                        )).ToList();

            return new Pagination<ProjectResponse>
            {
                Total = total,
                Size = items.Count,
                Items = responseItems
            };
        }

        public async Task<Pagination<ProjectResponse>> GetProjects(GetProjectsParameters paginationQuery)
        {
            IQueryable<Project> query = GetAvailable()
                .Include(p => p.Category)
                .Where(p => p.Name.ToLower().Contains(paginationQuery.SearchTerm))
                .OrderByDescending(p => p.Id);

            if (paginationQuery.Category != null)
            {
                query = query
                    .Where(p =>
                        p.Category != null &&
                        p.Category.Name.Contains(paginationQuery.Category, StringComparison.OrdinalIgnoreCase)
                    );
            }

            int total = await query.CountAsync();

            if (paginationQuery.IsCursorPagination)
            {
                query = query.Where(p => p.Id < paginationQuery.Cursor);
            }

            var items = await query.Take(paginationQuery.Limit)
                .Where(p => !dbContext.ProjectBans.Any(b => b.ProjectId == p.Id && b.IsActive == true))
                .Select(p => new
                {
                    p.Id,
                    p.PublicId,
                    p.Name,
                    p.Description,
                    Category = p.Category.Name,
                    p.FileLink,
                    p.ThumbnailLink,
                    Username = p.User.UserName,
                    p.LikeCount,
                    p.CreatedAt,
                    p.DeletedAt
                })
                .ToListAsync();

            var responseItems = items
                .Select(p => new ProjectResponse(
                    p.PublicId,
                    p.Name,
                    p.Description,
                    p.Category,
                    p.FileLink,
                    p.ThumbnailLink,
                    p.Username,
                    p.LikeCount,
                    false,
                    p.CreatedAt.ToString("o"),
                    p.DeletedAt?.ToString("o")
                )).ToList();

            return new Pagination<ProjectResponse>
            {
                Total = total,
                Size = items.Count,
                LastId = items.Count == 0 ? null : items[items.Count - 1].Id,
                Items = responseItems
            };
        }

        public async Task<int> GetUserProjectCount(int id)
        {
            return await dbContext.Projects.Where(p => p.UserId == id).CountAsync();
        }

        public async Task<Pagination<ProjectResponse>> GetUserProjects(int id, PaginationQuery paginationQuery)
        {
            IQueryable<Project> query = GetAvailable().Where(p => p.UserId == id);

            int total = await query.CountAsync();

            var items = await query.OrderByDescending(p => p.Id)
                .Skip(paginationQuery.Offset)
                .Take(paginationQuery.Limit)
                .Select(p => new
                {
                    p.Id,
                    p.PublicId,
                    p.Name,
                    p.Description,
                    Category = p.Category.Name,
                    p.FileLink,
                    p.ThumbnailLink,
                    Username = p.User.UserName,
                    p.LikeCount,
                    IsBanned = dbContext.ProjectBans.Any(b => b.ProjectId == p.Id && b.IsActive == true),
                    p.CreatedAt,
                    p.DeletedAt
                })
                .ToListAsync();

            var responseItems = items.Select(p => new ProjectResponse(
                                            p.PublicId,
                                            p.Name,
                                            p.Description,
                                            p.Category,
                                            p.FileLink,
                                            p.ThumbnailLink,
                                            p.Username,
                                            p.LikeCount,
                                            p.IsBanned,
                                            p.CreatedAt.ToString("o"),
                                            p.DeletedAt?.ToString("o")
                                        )).ToList();

            return new Pagination<ProjectResponse>
            {
                Total = total,
                Size = items.Count,
                LastId = items.LastOrDefault()?.Id,
                Items = responseItems
            };
        }

        public async Task<Pagination<ProjectResponse>> GetUserDeletedProjects(int id, PaginationQuery paginationQuery)
        {
            IQueryable<Project> query = GetDeleted().Where(p => p.UserId == id);

            int total = await query.CountAsync();

            var items = await query.OrderByDescending(p => p.Id)
                .Skip(paginationQuery.Offset)
                .Take(paginationQuery.Limit)
                .Select(p => new
                {
                    p.Id,
                    p.PublicId,
                    p.Name,
                    p.Description,
                    Category = p.Category.Name,
                    p.FileLink,
                    p.ThumbnailLink,
                    Username = p.User.UserName,
                    p.LikeCount,
                    IsBanned = dbContext.ProjectBans.Any(b => b.ProjectId == p.Id && b.IsActive == true),
                    p.CreatedAt,
                    p.DeletedAt
                })
                .ToListAsync();

            var responseItems = items.Select(p => new ProjectResponse(
                                            p.PublicId,
                                            p.Name,
                                            p.Description,
                                            p.Category,
                                            p.FileLink,
                                            p.ThumbnailLink,
                                            p.Username,
                                            p.LikeCount,
                                            p.IsBanned,
                                            p.CreatedAt.ToString("o"),
                                            p.DeletedAt?.ToString("o")
                                        )).ToList();

            return new Pagination<ProjectResponse>
            {
                Total = total,
                Size = items.Count,
                LastId = items.LastOrDefault()?.Id,
                Items = responseItems
            };
        }

        public async Task<Project> GetById(int id)
        {
            return await dbContext.Projects
                                    .Include(p => p.User)
                                    .Include(p => p.Category)
                                    .FirstAsync(p => p.Id == id);
        }

        public void Add(Project project)
        {
            dbContext.Projects.Add(project);
        }

        public async Task<int> GetProjectLike(int id)
        {
            var project = await dbContext.Projects.FindAsync(id);
            return project.LikeCount;
        }

        public void SoftDelete(Project project)
        {
            project.DeletedAt = DateTime.UtcNow;
        }
        public void Undelete(Project project)
        {
            project.DeletedAt = null;
        }

        public void Delete(Project project)
        {
            dbContext.Projects.Remove(project);
        }

        private IQueryable<Project> GetAvailable()
        {
            return dbContext.Projects.Where(
                p => !p.DeletedAt.HasValue &&
                !p.ProjectBans.Any(b => b.IsActive)
            );
        }

        private IQueryable<Project> GetDeleted()
        {
            return dbContext.Projects.Where(p => p.DeletedAt.HasValue);
        }


        //public async Task<Pagination<ProjectResponse>> GetProjectsOffset(int? page = null, int? limit = null)
        //{
        //    int pageSize = limit ?? _defaultLimit;
        //    int currentPage = page ?? 1;
        //    int offset = (currentPage - 1) * pageSize;

        //    IQueryable<Project> query = GetAvailable();

        //    int total = await query.CountAsync();

        //    var items = await query.OrderByDescending(p => p.Id)
        //        .Skip(offset)
        //        .Take(pageSize)
        //        .Select(p => new
        //        {
        //            p.Id,
        //            p.PublicId,
        //            p.Name,
        //            p.Description,
        //            Category = p.Category.Name,
        //            p.FileLink,
        //            p.ThumbnailLink,
        //            Username = p.User.UserName,
        //            p.LikeCount,
        //            IsBanned = dbContext.ProjectBans.Any(b => b.ProjectId == p.Id && b.IsActive == true),
        //            p.CreatedAt,
        //            p.DeletedAt
        //        })
        //        .ToListAsync();

        //    var responseItems = items.Select(p => new ProjectResponse(
        //                                    p.PublicId,
        //                                    p.Name,
        //                                    p.Description,
        //                                    p.Category,
        //                                    p.FileLink,
        //                                    p.ThumbnailLink,
        //                                    p.Username,
        //                                    p.LikeCount,
        //                                    p.IsBanned,
        //                                    p.CreatedAt.ToString("o"),
        //                                    p.DeletedAt?.ToString("o")
        //                                )).ToList();

        //    return new Pagination<ProjectResponse>
        //    {
        //        Total = total,
        //        Size = items.Count,
        //        LastId = items.LastOrDefault()?.Id,
        //        Items = responseItems
        //    };
        //}
    }
}

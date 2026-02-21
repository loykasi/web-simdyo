using Application.Interfaces.Repositories;
using Application.Models.Responses.ProjectComment;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Respositories
{
    public class ProjectCommentRepository(ApplicationDbContext dbContext) : IProjectCommentRepository
    {
        private const int _defaultLimit = 20;

        public async Task<int> Count()
        {
            return await dbContext.ProjectComments.Select(p => p.Id).CountAsync();
        }

        public async Task<Pagination<ProjectCommentResponse>> GetComments(int projectId, int? limit = null, int? lastId = null, int? parentId = null)
        {
            IQueryable<ProjectComment> query = dbContext.ProjectComments
                        .Include(p => p.User)
                        .Include(p => p.RepliedUser);

            bool isDescending = parentId == null;
            if (isDescending)
            {
                query = query.Where(p => p.ParentCommentId == null).OrderByDescending(p => p.Id);
            }
            else
            {
                query = query.Where(p => p.ParentCommentId == parentId).OrderBy(p => p.Id);
            }

            int total = await query.Select(p => p.Id).CountAsync();

            if (lastId != null)
            {
                if (isDescending)
                {
                    query = query.Where(p => p.Id < lastId);
                }
                else
                {
                    query = query.Where(p => p.Id > lastId);
                }
            }
            
            int size = limit == null || limit == 0 ? _defaultLimit : limit.Value;

            var items = await query.Take(size)
                            .Select(p =>
                                new ProjectCommentResponse
                                (
                                    p.Id,
                                    p.Content,
                                    p.ParentCommentId,
                                    p.User.UserName,
                                    p.RepliedUser != null ? p.RepliedUser.UserName : null,
                                    p.CreatedAt.ToString("o"),
                                    dbContext.ProjectComments.Where(c => c.ParentCommentId == p.Id).Select(c => c.Id).Count()
                                )
                            )
                            .ToListAsync();

            Pagination<ProjectCommentResponse> response = new()
            {
                Total = total,
                Size = items.Count,
                LastId = items.Count == 0 ? null : items[items.Count - 1].Id,
                Items = items
            };

            return response;
        }

        public async Task<ProjectComment> Get(int id)
        {
            return await dbContext.ProjectComments.FindAsync(id);
        }

        public void Add(ProjectComment projectComment)
        {
            dbContext.ProjectComments.Add(projectComment);
        }

        public async Task Delete(int id)
        {
            await dbContext.ProjectComments.Where(c => c.Id == id || c.ParentCommentId == id).ExecuteDeleteAsync();
        }
    }
}
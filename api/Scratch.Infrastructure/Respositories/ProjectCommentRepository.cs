using Microsoft.EntityFrameworkCore;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Entities;
using Scratch.Domain.Responses;
namespace Scratch.Infrastructure.Respositories
{
    public class ProjectCommentRepository(ApplicationDbContext dbContext) : IProjectCommentRepository
    {
        private const int _defaultLimit = 20;

        public async Task<List<ProjectCommentResponse>> GetComments(int projectId, int? limit = null, int? lastId = null, int? parentId = null)
        {
            IQueryable<ProjectComment> query = dbContext.ProjectComments
                        .Include(p => p.User)
                        .Include(p => p.RepliedUser);

            bool isDescending = parentId == null;
            if (isDescending)
            {
                query = query.Where(p => p.ParentId == null).OrderByDescending(p => p.Id);
            }
            else
            {
                query = query.Where(p => p.ParentId == parentId).OrderBy(p => p.Id);
            }

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
            
            int size = (limit == null || limit == 0) ? _defaultLimit : limit.Value;

            return await query.Take(size)
                            .Select(p =>
                                new ProjectCommentResponse
                                (
                                    p.Id,
                                    p.Content,
                                    p.ParentId,
                                    p.User.UserName,
                                    p.RepliedUser != null ? p.RepliedUser.UserName : null,
                                    p.CreatedAt.ToString("o"),
                                    dbContext.ProjectComments.Where(c => c.ParentId == p.Id).Select(c => c.Id).Count()
                                )
                            )
                            .ToListAsync();
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
            await dbContext.ProjectComments.Where(c => c.Id == id || c.ParentId == id).ExecuteDeleteAsync();
        }
    }
}
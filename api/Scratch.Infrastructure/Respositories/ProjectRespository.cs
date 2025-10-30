using Microsoft.EntityFrameworkCore;
using Scratch.Application.Abstracts;
using Scratch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratch.Infrastructure.Respositories
{
    public class ProjectRespository(ApplicationDbContext dbContext) : IProjectRepository
    {
        public async Task<IEnumerable<Project>> GetProjects()
        {
            return await dbContext.Projects.ToListAsync();
        }

        public async Task<Project> GetById(Guid id)
        {
            return await dbContext.Projects.FindAsync(id);
        }

        public void Add(Project project)
        {
            dbContext.Projects.Add(project);
        }
    }
}

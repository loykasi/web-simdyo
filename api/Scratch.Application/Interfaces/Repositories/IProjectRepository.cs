using Scratch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjects();
        Task<int> GetUserProjectCount(Guid id);
        Task<IEnumerable<Project>> GetUserProjects(Guid id);
        Task<IEnumerable<Project>> GetUserDeletedProjects(Guid id);
        Task<Project> GetById(int id);
        void Add(Project project);
        Task<int> GetProjectLike(int id);
        void SoftDelete(Project project);
        void Undelete(Project project);
        void Delete(Project project);
    }
}

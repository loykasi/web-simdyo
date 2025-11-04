using Scratch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratch.Application.Abstracts
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjects();
        Task<Project> GetById(int id);
        void Add(Project project);
        Task<int> GetTotalProjectFromAccount(Guid id);
        Task<IEnumerable<Project>> GetUserProjects(Guid id);
        Task<int> GetProjectLike(int id);
    }
}

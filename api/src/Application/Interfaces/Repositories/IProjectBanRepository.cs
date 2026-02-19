using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IProjectBanRepository
    {
        Task<ProjectBan?> GetByProjectId(int projectId);
        Task<bool> GetBanStatus(int projectId);
        void Add(ProjectBan projectBan);
    }
}

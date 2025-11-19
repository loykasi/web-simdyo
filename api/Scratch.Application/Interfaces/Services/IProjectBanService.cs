using Scratch.Domain.Requests;
using Scratch.Domain.Results;

namespace Scratch.Application.Interfaces.Services
{
    public interface IProjectBanService
    {
        Task<Result> AddBan(string publicId, BanProjectRequest payload);
        Task<Result> RevokeBan(string publicId);
    }
}

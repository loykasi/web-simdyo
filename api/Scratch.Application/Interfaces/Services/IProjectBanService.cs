using Scratch.Application.Models.Requests;
using Scratch.Application.Results;

namespace Scratch.Application.Interfaces.Services
{
    public interface IProjectBanService
    {
        Task<Result> AddBan(string publicId, BanProjectRequest payload);
        Task<Result> RevokeBan(string publicId);
    }
}

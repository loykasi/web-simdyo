using Application.Models.Requests;
using Application.Results;

namespace Application.Interfaces.Services
{
    public interface IProjectBanService
    {
        Task<Result> AddBan(string publicId, BanProjectRequest payload);
        Task<Result> RevokeBan(string publicId);
    }
}

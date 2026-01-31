using Scratch.Application.Models.Requests;
using Scratch.Application.Results;

namespace Scratch.Application.Interfaces.Services
{
    public interface IUserBanService
    {
        Task<Result> Ban(int userId, UserBanRequest payload);
        Task<Result> RevokeBan(int userId);
    }
}

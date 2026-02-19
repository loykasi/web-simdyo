using Application.Models.Requests;
using Application.Results;

namespace Application.Interfaces.Services
{
    public interface IUserBanService
    {
        Task<Result> Ban(int userId, UserBanRequest payload);
        Task<Result> RevokeBan(int userId);
    }
}

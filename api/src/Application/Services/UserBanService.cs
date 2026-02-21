using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Models.Requests.Account;
using Application.Results;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserBanService
    (
        IUnitOfWork unitOfWork,
        UserManager<User> userManager,
        ICurrentUserService currentUserService
    ) : IUserBanService
    {
        public async Task<Result> Ban(int userId, UserBanRequest payload)
        {
            string authorID = currentUserService.GetUserID();

            if (authorID == userId.ToString())
            {
                return Result.Failure
                (
                    new Error("Ban.Failure", "Can not ban yourself")
                );
            }

            var user = await userManager.FindByIdAsync(authorID);
            if (user == null)
            {
                return Result.NotFound
                (
                    new Error("Users.NotFound", "Invalid token")
                );
            }

            var targetUser = await userManager.FindByIdAsync(userId.ToString());
            if (targetUser == null)
            {
                return Result.NotFound
                (
                    new Error("Users.NotFound", "Invalid token")
                );
            }

            var isBanned = await unitOfWork.UserBanRepository.GetBanStatus(userId);
            if (isBanned)
            {
                return Result.Success();
            }

            UserBan userBan = new()
            {
                Reason = payload.Reason,
                Description = payload.Description,
                User = targetUser,
                ByUser = user,
                IsActive = true
            };

            unitOfWork.UserBanRepository.Add(userBan);
            await unitOfWork.SaveChangesAsync();
            
            return Result.Success();
        }

        public async Task<Result> RevokeBan(int userId)
        {
            string adminId = currentUserService.GetUserID();
            var admin = await userManager.FindByIdAsync(adminId);
            if (admin == null)
            {
                return Result.NotFound
                (
                    new Error("Users.NotFound", "Invalid token")
                );
            }

            var userBan = await unitOfWork.UserBanRepository.GetByUserId(userId);

            if (userBan == null)
            {
                return Result.Success();
            }

            userBan.IsActive = false;
            userBan.RevokedByUser = admin;

            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}

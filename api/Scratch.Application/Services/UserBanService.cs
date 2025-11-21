using Microsoft.AspNetCore.Identity;
using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Application.Interfaces.Services;
using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.Application.Services
{
    public class UserBanService
    (
        IUnitOfWork unitOfWork,
        UserManager<User> userManager,
        ICurrentUserService currentUserService
    ) : IUserBanService
    {
        public async Task<Result> Ban(Guid userId, UserBanRequest payload)
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

        public async Task<Result> RevokeBan(Guid userId)
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

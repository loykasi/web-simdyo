using Microsoft.AspNetCore.Identity;
using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Services;
using Scratch.Domain.Entities;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.Application.Services
{
    public class PermissionService(
        IUnitOfWork unitOfWork,
        UserManager<User> userManager
    ) : IPermissionService
    {
        public async Task<Result<string[]>> GetUserPermissionsAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.NotFound<string[]>
                (
                    new Error("User.NotFound", "Invalid token")
                );
            }

            var permissions = await unitOfWork.UserRespository.GetUserPermissionsAsync(user);
            return Result.Success(permissions);
        }
    }
}

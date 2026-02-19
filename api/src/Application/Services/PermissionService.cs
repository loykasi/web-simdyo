using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
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

using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Models.Requests.Role;
using Application.Models.Responses.Account;
using Application.Models.Responses.Project;
using Application.Results;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserService
    (
        IUnitOfWork unitOfWork,
        UserManager<User> userManager
    ): IUserService
    {
        public async Task<Result<Pagination<UserDto>>> Get(
            string? searchTerm, int? pageNumber = null, int? limit = null)
        {
            var pagination = await unitOfWork.UserRespository.Get(searchTerm, pageNumber, limit);
            return Result.Success(pagination);
        }

        public async Task<Result> SetRole(string id, SetUserRoleRequest payload)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return Result.NotFound<Pagination<ProjectResponse>>
                (
                    new Error("NotFound", "Invalid user Id")
                );
            }

            await unitOfWork.UserRespository.SetUserRole(user, payload.Roles);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Services;
using Scratch.Domain.DTO;
using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.Application.Services
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

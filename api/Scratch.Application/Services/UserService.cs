using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Services;
using Scratch.Domain.DTO;
using Scratch.Domain.Entities;
using Scratch.Domain.Results;

namespace Scratch.Application.Services
{
    public class UserService
    (
        IUnitOfWork unitOfWork
    ): IUserService
    {
        public async Task<Result<Pagination<UserDto>>> Get(int? pageNumber = null, int? limit = null)
        {
            var pagination = await unitOfWork.UserRespository.Get(pageNumber, limit);
            return Result.Success(pagination);
        }
    }
}

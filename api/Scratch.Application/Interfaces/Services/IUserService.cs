using Scratch.Domain.DTO;
using Scratch.Domain.Entities;
using Scratch.Domain.Results;

namespace Scratch.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<Pagination<UserDto>>> Get(int? pageNumber = null, int? limit = null);
    }
}

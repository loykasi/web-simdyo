using Scratch.Domain.DTO;
using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Results;

namespace Scratch.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<Pagination<UserDto>>> Get(string? searchTerm, int? pageNumber = null, int? limit = null);
        Task<Result> SetRole(string id, SetUserRoleRequest payload);
    }
}

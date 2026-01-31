using Scratch.Domain.Entities;
using Scratch.Application.Models.Requests;
using Scratch.Application.Results;
using Scratch.Application.Models.Responses;

namespace Scratch.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<Pagination<UserDto>>> Get(string? searchTerm, int? pageNumber = null, int? limit = null);
        Task<Result> SetRole(string id, SetUserRoleRequest payload);
    }
}

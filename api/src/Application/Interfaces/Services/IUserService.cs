using Application.Results;
using Domain.Entities;
using Application.Models.Requests.Role;
using Application.Models.Responses.Account;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<Pagination<UserDto>>> Get(string? searchTerm, int? pageNumber = null, int? limit = null);
        Task<Result> SetRole(string id, SetUserRoleRequest payload);
    }
}

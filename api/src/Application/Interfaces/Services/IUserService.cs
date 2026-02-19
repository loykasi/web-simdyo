using Application.Results;
using Application.Models.Responses;
using Application.Models.Requests;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<Pagination<UserDto>>> Get(string? searchTerm, int? pageNumber = null, int? limit = null);
        Task<Result> SetRole(string id, SetUserRoleRequest payload);
    }
}

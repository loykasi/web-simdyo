using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Application.Interfaces.Services;
using Scratch.Domain.Authorizations;
using Scratch.Domain.Dto;
using Scratch.Domain.Requests;
using Scratch.Domain.Results;

namespace Scratch.Application.Services
{
    public class RoleService
    (
        IUnitOfWork unitOfWork
    ): IRoleService
    {
        public async Task<Result<RoleDto[]>> GetAll()
        {
            return Result.Success(await unitOfWork.RoleRepository.GetAll());
        }

        public async Task<Result> UpdateRolePermissions(Guid roleId, UpdateRolePermissionsRequest payload)
        {
            if (payload.Enables != null && payload.Enables.Length > 0)
            {
                await unitOfWork.RoleRepository.AddPermissions(roleId, payload.Enables);
            }
            if (payload.Disables != null && payload.Disables.Length > 0)
            {
                await unitOfWork.RoleRepository.DeletePermissions(roleId, payload.Disables);
            }

            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}

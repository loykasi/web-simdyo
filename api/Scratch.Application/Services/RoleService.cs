using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Application.Interfaces.Services;
using Scratch.Domain.Authorizations;
using Scratch.Domain.Dto;
using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Results;

namespace Scratch.Application.Services
{
    public class RoleService(IUnitOfWork unitOfWork): IRoleService
    {
        public async Task<Result<RoleDto[]>> GetAll()
        {
            return Result.Success(await unitOfWork.RoleRepository.GetAll());
        }
        public async Task<Result<string[]>> GetAllName()
        {
            return Result.Success(await unitOfWork.RoleRepository.GetAllName());
        }

        public async Task<Result<RoleDto>> Add(AddRoleRequest payload)
        {
            var exist = await unitOfWork.RoleRepository.IsRoleExist(payload.Name);
            if (exist)
            {
                return Result.Conflict<RoleDto>
                (
                    new Error("Role.Exist", $"Role {payload.Name} is already exist")
                );
            }

            Role role = new(payload.Name)
            {
                NormalizedName = payload.Name.Normalize().ToUpper()
            };
            await unitOfWork.RoleRepository.AddRole(role);
            //await unitOfWork.SaveChangesAsync();

            return Result.Success
            (
                new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name!,
                    Permissions = []
                }
            );
        }

        public async Task<Result> Update(int id, UpdateRoleRequest payload)
        {
            var role = await unitOfWork.RoleRepository.GetRoleById(id);
            if (role == null)
            {
                return Result.Conflict<RoleDto>
                (
                    new Error("Role.NotFound", $"Role with id {id} not found")
                );
            }

            role.Name = payload.Name;
            role.NormalizedName = payload.Name.Normalize();

            if (payload.Enables != null && payload.Enables.Length > 0)
            {
                await unitOfWork.RoleRepository.AddPermissions(id, payload.Enables);
            }
            if (payload.Disables != null && payload.Disables.Length > 0)
            {
                await unitOfWork.RoleRepository.DeletePermissions(id, payload.Disables);
            }

            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> Delete(int id)
        {
            await unitOfWork.RoleRepository.DeleteRole(id);
            return Result.Success();
        }

        public async Task<Result> UpdateRolePermissions(int roleId, UpdateRolePermissionsRequest payload)
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

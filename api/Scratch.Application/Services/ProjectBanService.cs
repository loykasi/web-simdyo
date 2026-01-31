using Microsoft.AspNetCore.Identity;
using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Services;
using Scratch.Application.Models.Requests;
using Scratch.Application.Results;
using Scratch.Domain.Entities;

namespace Scratch.Application.Services
{
    public class ProjectBanService
    (
        IUnitOfWork unitOfWork,
        IPublicIdService publicIdService,
        UserManager<User> userManager,
        ICurrentUserService currentUserService
    ) : IProjectBanService
    {
        public async Task<Result> AddBan(string publicId, BanProjectRequest payload)
        {
            string userId = currentUserService.GetUserID();
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.NotFound
                (
                    new Error("Users.NotFound", "Invalid token")
                );
            }

            if (!DecodeId(publicId, out int projectId))
            {
                return Result.NotFound
                (
                    new Error("Project.InvalidPublicId", "Invalid public ID.")
                );
            }

            var project = await unitOfWork.ProjectRepository.GetById(projectId);
            if (project == null)
            {
                return Result.NotFound
                (
                    new Error("Project.NotFound", $"No project with id: {publicId}.")
                );
            }

            ProjectBan ban = new()
            {
                Reason = payload.Reason,
                Description = payload.Description,
                Project = project,
                ByUser = user,
                IsActive = true
            };

            unitOfWork.ProjectBanRepository.Add(ban);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> RevokeBan(string publicId)
        {
            string userId = currentUserService.GetUserID();
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.NotFound
                (
                    new Error("Users.NotFound", "Invalid token")
                );
            }

            if (!DecodeId(publicId, out int projectId))
            {
                return Result.NotFound
                (
                    new Error("Project.InvalidPublicId", "Invalid public ID.")
                );
            }

            var project = await unitOfWork.ProjectRepository.GetById(projectId);
            if (project == null)
            {
                return Result.NotFound
                (
                    new Error("Project.NotFound", $"No project with id: {publicId}.")
                );
            }

            var ban = await unitOfWork.ProjectBanRepository.GetByProjectId(project.Id);
            if (ban == null)
            {
                return Result.Success();
            }

            ban.IsActive = false;
            ban.RevokedByUser = user;
            
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        private bool DecodeId(string publicId, out int id)
        {
            id = 0;
            if (publicIdService.Decode(publicId) is [int decodedId] &&
                publicId == publicIdService.Encode(decodedId))
            {
                id = decodedId;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

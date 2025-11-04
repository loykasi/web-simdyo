using Microsoft.AspNetCore.Identity;
using Scratch.Application.Abstracts;
using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.Application.Services
{
    public class ProjectLikeService
    (
        UserManager<User> userManager,
        IUnitOfWork unitOfWork,
        IPublicIdService publicIdService,
        ICurrentUserService currentUserService
    ) : IProjectLikeService
    {
        public async Task<Result<int>> GetLike(string projectPublicId)
        {
            if (!DecodeId(projectPublicId, out int projectId))
            {
                return Result.NotFound<int>
                (
                    new Error("Project.NotFound", "Not found.")
                );
            }

            Project project = await unitOfWork.ProjectRepository.GetById(projectId);

            if (project == null)
            {
                return Result.NotFound<int>
                (
                    new Error("Project.NotFound", "Not found.")
                );
            }

            return Result.Success(project.LikeCount);
        }

        public async Task<Result<bool>> GetLikeStatus(string projectPublicId)
        {
            string userID = currentUserService.GetUserID();

            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return Result.NotFound<bool>
                (
                    new Error("User.NotFound", "Invalid token.")
                );
            }

            if(!DecodeId(projectPublicId, out int projectId))
            {
                return Result.NotFound<bool>
                (
                    new Error("Project.InvalidPublicId", "Invalid Public Id.")
                );
            }

            bool result = await unitOfWork.ProjectLikeRepository.Exist(projectId, user.Id);

            return Result.Success(result);
        }

        public async Task<Result> AddLike(string projectPublicId)
        {
            string userID = currentUserService.GetUserID();

            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return Result.NotFound
                (
                    new Error("User.NotFound", "Invalid token.")
                );
            }

            if (!DecodeId(projectPublicId, out int projectId))
            {
                return Result.NotFound
                (
                    new Error("Project.InvalidPublicId", "Invalid Public Id.")
                );
            }

            Project project = await unitOfWork.ProjectRepository.GetById(projectId);
            
            if (project == null)
            {
                return Result.NotFound
                (
                    new Error("Project.NotFound", "Project not found.")
                );
            }

            bool exist = await unitOfWork.ProjectLikeRepository.Exist(projectId, user.Id);
            if (exist)
            {
                return Result.Success();
            }

            ProjectLike projectLike = new()
            {
                Project = project,
                User = user
            };

            project.LikeCount++;

            unitOfWork.ProjectLikeRepository.Add(projectLike);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteLike(string projectPublicId)
        {
            string userID = currentUserService.GetUserID();

            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return Result.NotFound
                (
                    new Error("User.NotFound", "Invalid token.")
                );
            }

            if (!DecodeId(projectPublicId, out int projectId))
            {
                return Result.NotFound
                (
                    new Error("Project.InvalidPublicId", "Invalid public ID.")
                );
            }

            Project project = await unitOfWork.ProjectRepository.GetById(projectId);

            if (project == null)
            {
                return Result.NotFound
                (
                    new Error("Project.NotFound", "Project not found.")
                );
            }

            ProjectLike projectLike = await unitOfWork.ProjectLikeRepository.Get(projectId, user.Id);

            if (projectLike == null)
            {
                return Result.NotFound
                (
                    new Error("Like.NotFound", "Like not found.")
                );
            }

            unitOfWork.ProjectLikeRepository.Delete(projectLike);

            project.LikeCount--;

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

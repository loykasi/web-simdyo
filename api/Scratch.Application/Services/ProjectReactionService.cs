using Microsoft.AspNetCore.Identity;
using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Services;
using Scratch.Domain.Entities;
using Scratch.Domain.Enums;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.Application.Services
{
    public class ProjectReactionService
    (
        UserManager<User> userManager,
        IUnitOfWork unitOfWork,
        IPublicIdService publicIdService,
        ICurrentUserService currentUserService
    ) : IProjectReactionService
    {
        //public async Task<Result<ProjectReactionResponse>> GetReactionCount(string publicId)
        //{
        //    Project? project = await TryGetProject(publicId);

        //    if (project == null)
        //    {
        //        return Result.NotFound<ProjectReactionResponse>
        //        (
        //            new Error("Project.NotFound", "Not found.")
        //        );
        //    }

        //    return Result.Success(project.LikeCount);
        //}

        public async Task<Result<string>> GetReactionStatus(string publicId)
        {
            var user = await currentUserService.GetUserAsync();
            if (user == null)
            {
                return Result.NotFound<string>
                (
                    new Error("User.NotFound", "Invalid token.")
                );
            }

            if(!DecodeId(publicId, out int projectId))
            {
                return Result.NotFound<string>
                (
                    new Error("Project.InvalidId", "Invalid Id.")
                );
            }

            string type = await unitOfWork.ProjectReactionRepository.GetReactionType(projectId, user.Id);
            return Result.Success(type);
        }

        public async Task<Result> AddReaction(string publicId, AddReactionRequest request)
        {
            Console.WriteLine($"ID ============== {request.Type}");
            var user = await currentUserService.GetUserAsync();
            if (user == null)
            {
                return Result.NotFound
                (
                    new Error("User.NotFound", "Invalid token.")
                );
            }

            Project? project = await GetProject(publicId);
            if (project == null)
            {
                return Result.NotFound
                (
                    new Error("Project.NotFound", "Project not found.")
                );
            }

            ProjectReaction? reaction = await unitOfWork.ProjectReactionRepository.Get(project.Id, user.Id);
            if (reaction != null)
            {
                reaction.Type = request.Type;
                await unitOfWork.SaveChangesAsync();
                return Result.Success();
            }

            reaction = new()
            {
                Type = request.Type,
                Project = project,
                User = user
            };

            unitOfWork.ProjectReactionRepository.Add(reaction);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteReaction(string publicId)
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

            Project? project = await GetProject(publicId);
            if (project == null)
            {
                return Result.NotFound
                (
                    new Error("Project.NotFound", "Project not found.")
                );
            }

            ProjectReaction? reaction = await unitOfWork.ProjectReactionRepository.Get(project.Id, user.Id);
            if (reaction == null)
            {
                return Result.Success();
            }

            unitOfWork.ProjectReactionRepository.Delete(reaction);
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

        private async Task<Project?> GetProject(string publicId)
        {
            if (publicIdService.Decode(publicId) is [int decodedId] &&
                publicId == publicIdService.Encode(decodedId))
            {
                return await unitOfWork.ProjectRepository.GetById(decodedId);
            }

            return null;
        }
    }
}

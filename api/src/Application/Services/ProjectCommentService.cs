using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Models.Requests;
using Application.Models.Responses;
using Application.Results;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class ProjectCommentService
    (
        UserManager<User> userManager,
        IUnitOfWork unitOfWork,
        IPublicIdService publicIdService,
        ICurrentUserService currentUserService
    ) : IProjectCommentService
    {
        public async Task<Result<Pagination<ProjectCommentResponse>>> GetComments(string projectPublicId, int? limit = null, int? lastId = null, int? parentId = null)
        {
            if (!DecodeId(projectPublicId, out int projectId))
            {
                return Result.NotFound<Pagination<ProjectCommentResponse>>
                (
                    new Error("Project.InvalidPublicId", "Invalid Public Id.")
                );
            }

            var response = await unitOfWork.ProjectCommentRepository.GetComments(projectId, limit, lastId, parentId);
            return Result.Success(response);
        }

        public async Task<Result<ProjectCommentResponse>> Add(string projectPublicId, AddCommentRequest addCommentRequest)
        {
            string userID = currentUserService.GetUserID();

            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return Result.NotFound<ProjectCommentResponse>
                (
                    new Error("User.NotFound", "Invalid token.")
                );
            }

            if (!DecodeId(projectPublicId, out int projectId))
            {
                return Result.NotFound<ProjectCommentResponse>
                (
                    new Error("Project.InvalidPublicId", "Invalid Public Id.")
                );
            }

            Project project = await unitOfWork.ProjectRepository.GetById(projectId);

            if (project == null)
            {
                return Result.NotFound<ProjectCommentResponse>
                (
                    new Error("Project.NotFound", "Project not found.")
                );
            }

            User? repliedUser = null;
            if (addCommentRequest.RepliedUsername != null)
            {
                repliedUser = await userManager.FindByNameAsync(addCommentRequest.RepliedUsername);    
            }
            

            ProjectComment comment = new()
            {
                Content = addCommentRequest.Content,
                User = user,
                Project = project,
                ParentCommentId = addCommentRequest.ParentId,
                RepliedUser = repliedUser ?? null,
            };
            
            unitOfWork.ProjectCommentRepository.Add(comment);
            await unitOfWork.SaveChangesAsync();

            return Result.Success
            (
                new ProjectCommentResponse
                (
                    comment.Id,
                    comment.Content,
                    comment.ParentCommentId,
                    comment.User.UserName,
                    comment.RepliedUser != null ? comment.RepliedUser.UserName : null,
                    comment.CreatedAt.ToString("o"),
                    0
                )
            );
        }

        public async Task<Result> Remove(string projectPublicId, int commentId)
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

            ProjectComment comment = await unitOfWork.ProjectCommentRepository.Get(commentId);

            if (comment.UserId != user.Id)
            {
                return Result.UnAuthorized
                (
                    new Error("Comment.UnAuthorized", "Unauthorized.")
                );
            }

            await unitOfWork.ProjectCommentRepository.Delete(commentId);
            
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
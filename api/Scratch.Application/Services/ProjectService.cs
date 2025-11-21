using Microsoft.AspNetCore.Identity;
using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Entities;
using Scratch.Domain.Extensions;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.Application.Services
{
    public class ProjectService
    (
        IUnitOfWork unitOfWork,
        IObjectStorageService objectStorageService,
        ICurrentUserService currentUserService,
        UserManager<User> userManager,
        IPublicIdService publicIdService
    ) : IProjectService
    {
        public async Task<Result<Pagination<ProjectResponse>>> GetAll(int? page = null, int? limit = null)
        {
            var response = await unitOfWork.ProjectRepository.GetAllProjects(page, limit);
            return Result.Success(response);
        }

        public async Task<Result<Pagination<ProjectResponse>>> GetProjectsAsync(
            int? cursor = null,
            int? page = null,
            int? limit = null
        )
        {
            Pagination<ProjectResponse> response;
            if (page.HasValue)
            {
                response = await unitOfWork.ProjectRepository.GetProjectsOffset(page, limit);
            }
            else
            {
                response = await unitOfWork.ProjectRepository.GetProjectsCursor(cursor, limit);
            }
                
            return Result.Success(response);
        }

        public async Task<Result<ProjectsResponse>> GetUserProjects(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return Result.NotFound<ProjectsResponse>
                (
                    new Error("Profile.NotFound", "Invalid token")
                );
            }

            var projects = await unitOfWork.ProjectRepository.GetUserProjects(user.Id);

            var dto = projects.ToList();

            return Result.Success
            (
                new ProjectsResponse(dto)
            );
        }

        public async Task<Result<ProjectsResponse>> GetUserTrashAsync()
        {
            string userID = currentUserService.GetUserID();
            var user = await userManager.FindByIdAsync(userID);

            if (user == null)
            {
                return Result.NotFound<ProjectsResponse>
                (
                    new Error("Profile.NotFound", "Invalid token")
                );
            }

            var projects = await unitOfWork.ProjectRepository.GetUserDeletedProjects(user.Id);

            var dto = projects.ToList();

            return Result.Success
            (
                new ProjectsResponse(dto)
            );
        }

        public async Task<Result<ProjectResponse>> Get(string publicId)
        {
            if (!DecodeId(publicId, out int projectId))
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.InvalidPublicId", "Invalid public ID.")
                );
            }

            var project = await unitOfWork.ProjectRepository.GetById(projectId);
            if (project == null)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.NotFound", $"No project with id: {publicId}.")
                );
            }

            //if (project.DeletedAt.HasValue)
            //{
            //    return Result.Success(project.ToProjectResponse());
            //}

            //return Result.Success(project.ToProjectResponse());

            var isBanned = await unitOfWork.ProjectBanRepository.GetBanStatus(project.Id);

            if (currentUserService.HasValidAccessToken())
            {
                string userID = currentUserService.GetUserID();
                var user = await userManager.FindByIdAsync(userID);

                if (isBanned)
                {
                    return Result.Success
                    (
                        new ProjectResponse
                        (
                            project.PublicId,
                            project.Name,
                            project.Description,
                            project.Category.Name,
                            string.Empty,
                            string.Empty,
                            project.User.UserName,
                            project.LikeCount,
                            true,
                            project.CreatedAt.ToString("o"),
                            project.DeletedAt.HasValue ? project.DeletedAt.Value.ToString("o") : null
                        )
                    );
                }

                if (user == null)
                {
                    return Result.NotFound<ProjectResponse>
                    (
                        new Error("Profile.InvalidUserToken", "Invalid token")
                    );
                }

                if (project.UserId == user.Id)
                {
                    return Result.Success(project.ToProjectResponse());
                }

                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.NotFound", $"No project with id: {publicId}.")
                );
            }
            else
            {
                if (project.DeletedAt.HasValue || isBanned)
                {
                    return Result.NotFound<ProjectResponse>
                    (
                        new Error("Project.NotFound", $"No project with id: {publicId}.")
                    );
                }

                return Result.Success(project.ToProjectResponse());
            }
        }

        public async Task<Result<ProjectResponse>> Upload(UploadProjectRequest addProjectRequest)
        {
            string userID = currentUserService.GetUserID();

            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Profile.InvalidUserToken", "Invalid token")
                );
            }

            var category = await unitOfWork.ProjectCategoryRepository.GetByName(addProjectRequest.Category);
            if (category == null)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Category.Invalid", "Invalid category")
                );
            }

            Project project = new()
            {
                Name = addProjectRequest.Title,
                Description = addProjectRequest.Description,
                Category = category,
                User = user
            };
            unitOfWork.ProjectRepository.Add(project);

            string id = publicIdService.Encode(project.Id);

            project.PublicId = id;
            project.FileLink = await objectStorageService.Save(id + ".zip", addProjectRequest.ProjectFile);
            project.ThumbnailLink = await objectStorageService.Save(id + ".png", addProjectRequest.ThumbnailFile);

            await unitOfWork.SaveChangesAsync();

            return Result.Success(project.ToProjectResponse());
        }

        public async Task<Result<ProjectResponse>> Update(string publicId, UpdateProjectRequest updateProjectRequest)
        {
            string userID = currentUserService.GetUserID();

            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Profile.InvalidUserToken", "Invalid token")
                );
            }

            if (!DecodeId(publicId, out int projectId))
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.InvalidPublicId", "Invalid public ID.")
                );
            }

            var project = await unitOfWork.ProjectRepository.GetById(projectId);
            if (project == null)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.NotFound", $"No project with id: {publicId}.")
                );
            }

            if (project.UserId != user.Id)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.Unauthorized", $"Cannot delete project with id: {publicId}.")
                );
            }

            var category = await unitOfWork.ProjectCategoryRepository.GetByName(updateProjectRequest.Category);
            if (category == null)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Category.Invalid", "Invalid category")
                );
            }
            
            project.Name = updateProjectRequest.Title;
            project.Description = updateProjectRequest.Description;
            project.Category = category;

            string name = project.PublicId;

            if (updateProjectRequest.ProjectFile != null)
            {
                await objectStorageService.Save(name + ".zip", updateProjectRequest.ProjectFile);
            }

            if (updateProjectRequest.ThumbnailFile != null)
            {
                await objectStorageService.Save(name + ".png", updateProjectRequest.ThumbnailFile);
            }

            await unitOfWork.SaveChangesAsync();

            return Result.Success(project.ToProjectResponse());
        }

        public async Task<Result> Delete(string publicId)
        {
            string userID = currentUserService.GetUserID();

            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return Result.NotFound
                (
                    new Error("Profile.InvalidUserToken", "Invalid token")
                );
            }

            if (!DecodeId(publicId, out int projectId))
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.InvalidPublicId", "Invalid public ID.")
                );
            }

            var project = await unitOfWork.ProjectRepository.GetById(projectId);
            if (project == null)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.NotFound", $"No project with id: {publicId}.")
                );
            }

            if (project.UserId != user.Id)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.Unauthorized", $"Cannot delete project with id: {publicId}.")
                );
            }

            unitOfWork.ProjectRepository.SoftDelete(project);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> Undelete(string publicId)
        {
            string userID = currentUserService.GetUserID();

            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return Result.NotFound
                (
                    new Error("Profile.InvalidUserToken", "Invalid token")
                );
            }

            if (!DecodeId(publicId, out int projectId))
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.InvalidPublicId", "Invalid public ID.")
                );
            }

            var project = await unitOfWork.ProjectRepository.GetById(projectId);
            if (project == null)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.NotFound", $"No project with id: {publicId}.")
                );
            }

            if (project.UserId != user.Id)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.Unauthorized", $"Cannot delete project with id: {publicId}.")
                );
            }

            unitOfWork.ProjectRepository.Undelete(project);
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

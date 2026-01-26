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
        public async Task<Result<Pagination<ProjectResponse>>> GetAll(
            string? filter,
            int? page = null,
            int? limit = null
        )
        {
            var response = await unitOfWork.ProjectRepository.GetAllProjects(filter, page, limit);
            return Result.Success(response);
        }

        public async Task<Result<Pagination<ProjectResponse>>> GetProjectsAsync(GetProjectsQuery query)
        {
            var response = await unitOfWork.ProjectRepository.GetProjects(query);

            return Result.Success(response);
        }

        public async Task<Result<Pagination<ProjectResponse>>> GetUserProjects(string userName, PaginationQuery parameters)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return Result.NotFound<Pagination<ProjectResponse>>
                (
                    new Error("Profile.NotFound", "Invalid token")
                );
            }

            var result = await unitOfWork.ProjectRepository.GetUserProjects(user.Id, parameters);

            return Result.Success(result);
        }

        public async Task<Result<Pagination<ProjectResponse>>> GetUserTrashAsync(PaginationQuery parameter)
        {
            string userID = currentUserService.GetUserID();
            var user = await userManager.FindByIdAsync(userID);

            if (user == null)
            {
                return Result.NotFound<Pagination<ProjectResponse>>
                (
                    new Error("Profile.NotFound", "Invalid token")
                );
            }

            var result = await unitOfWork.ProjectRepository.GetUserDeletedProjects(user.Id, parameter);

            return Result.Success(result);
        }

        public async Task<Result<ProjectResponse>> Get(string publicId)
        {
            if (!publicIdService.TryDecodeId(publicId, out int projectId))
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.InvalidPublicId", "Invalid public ID.")
                );
            }
            
            var project = await unitOfWork.ProjectRepository.GetDetailById(projectId);
            if (project == null)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.NotFound", $"No project with id: {publicId}.")
                );
            }

            //var isBanned = await unitOfWork.ProjectBanRepository.GetBanStatus(project.Id);
            var user = await currentUserService.GetUserAsync();

            if (user != null)
            {
                if (user != null && project.Username == user.UserName && project.IsBanned)
                {
                    return Result.Success(project);
                }

                return Result.Success(project);
            }

            if (!string.IsNullOrEmpty(project.DeletedAt) || project.IsBanned)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.NotFound", $"No project with id: {publicId}.")
                );
            }

            return Result.Success(project);
        }

        public async Task<Result<ProjectResponse>> Upload(UploadProjectRequest addProjectRequest)
        {
            var user = await currentUserService.GetUserAsync();
            if (user == null)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Profile.InvalidUserToken", "Invalid token")
                );
            }

            ProjectCategory? category = null;
            if (addProjectRequest.Category != null)
            {
                category = await unitOfWork.ProjectCategoryRepository.GetByName(addProjectRequest.Category);
                if (category == null)
                {
                    return Result.NotFound<ProjectResponse>
                    (
                        new Error("Category.Invalid", "Invalid category")
                    );
                }
            }

            Project project = new()
            {
                Name = addProjectRequest.Title,
                ShortDescription = addProjectRequest.ShortDescription,
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

        public async Task<Result> Update(string publicId, UpdateProjectRequest updateProjectRequest)
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

            var project = await publicIdService.GetProject(publicId);
            if (project == null)
            {
                return Result.NotFound
                (
                    new Error("Project.NotFound", $"No project with id: {publicId}.")
                );
            }

            if (project.UserId != user.Id)
            {
                return Result.NotFound
                (
                    new Error("Project.Unauthorized", $"Cannot delete project with id: {publicId}.")
                );
            }
            
            project.Name = updateProjectRequest.Title;
            project.ShortDescription = updateProjectRequest.ShortDescription;
            project.Description = updateProjectRequest.Description;

            if (updateProjectRequest.Category != null)
            {
                var category = await unitOfWork.ProjectCategoryRepository.GetByName(updateProjectRequest.Category);
                if (category == null)
                {
                    return Result.NotFound
                    (
                        new Error("Category.Invalid", "Invalid category")
                    );
                }

                project.Category = category;
            }

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

            return Result.Success();
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

            var project = await publicIdService.GetProject(publicId);
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

            var project = await publicIdService.GetProject(publicId);
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
    }
}

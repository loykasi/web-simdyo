using Microsoft.AspNetCore.Identity;
using Scratch.Application.Abstracts;
using Scratch.Domain.Entities;
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
        public async Task<Result<ProjectsResponse>> GetProjectsAsync()
        {
            var projects = await unitOfWork.ProjectRepository.GetProjects();
            var dto = projects.Select(p =>
                new ProjectResponse(p.PublicId, p.Name, p.Description, p.Category, p.FileLink, p.ThumbnailLink, p.User.UserName)
            ).ToList();
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
                    new Error("Project.NotFound", $"No project with id: {projectId}.")
                );
            }

            return Result.Success
            (
                new ProjectResponse
                (
                    project.PublicId,
                    project.Name,
                    project.Description,
                    project.Category,
                    project.FileLink,
                    project.ThumbnailLink,
                    project.User.UserName
                )
            );
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

            Project project = new()
            {
                Name = addProjectRequest.Title,
                Description = addProjectRequest.Description,
                Category = addProjectRequest.Category,
                User = user
            };
            unitOfWork.ProjectRepository.Add(project);

            string id = publicIdService.Encode(project.Id);

            project.PublicId = id;
            project.FileLink = await objectStorageService.Save(id + ".zip", addProjectRequest.ProjectFile);
            project.ThumbnailLink = await objectStorageService.Save(id + ".png", addProjectRequest.ThumbnailFile);

            await unitOfWork.SaveChangesAsync();

            return Result.Success
            (
                new ProjectResponse
                (
                    project.PublicId,
                    project.Name,
                    project.Description,
                    project.Category,
                    project.FileLink,
                    project.ThumbnailLink,
                    project.User.UserName
                )
            );
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

            var dto = projects.Select(p => 
                new ProjectResponse(p.PublicId, p.Name, p.Description, p.Category, p.FileLink, p.ThumbnailLink, p.User.UserName)
            ).ToList();

            return Result.Success
            (
                new ProjectsResponse(dto)
            );
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

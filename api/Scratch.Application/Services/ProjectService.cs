using Microsoft.AspNetCore.Identity;
using Scratch.Application.Abstracts;
using Scratch.Domain.Entities;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratch.Application.Services
{
    public class ProjectService
    (
        IUnitOfWork unitOfWork,
        IObjectStorageService objectStorageService,
        ICurrentUserService currentUserService,
        UserManager<User> userManager
    ) : IProjectService
    {
        public async Task<Result<GetProjectsResponse>> GetProjectsAsync()
        {
            var projects = await unitOfWork.ProjectRepository.GetProjects();
            return Result.Success
            (
                new GetProjectsResponse(projects.ToList())
            );
        }

        public async Task<Result<ProjectResponse>> Get(Guid id)
        {
            var project = await unitOfWork.ProjectRepository.GetById(id);
            if (project == null)
            {
                return Result.NotFound<ProjectResponse>
                (
                    new Error("Project.NotFound", $"No project with id: {id}.")
                );
            }

            return Result.Success
            (
                new ProjectResponse
                (
                    project.Id,
                    project.Name,
                    project.Description,
                    project.Category,
                    project.FileLink,
                    project.ThumbnailLink
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

            string id = project.Id.ToString();

            project.FileLink = await objectStorageService.Save(id + ".zip", addProjectRequest.ProjectFile);
            project.ThumbnailLink = await objectStorageService.Save(id + ".png", addProjectRequest.ThumbnailFile);

            await unitOfWork.SaveChangesAsync();

            return Result.Success
            (
                new ProjectResponse
                (
                    project.Id,
                    project.Name,
                    project.Description,
                    project.Category,
                    project.FileLink,
                    project.ThumbnailLink
                )
            );
        }
    }
}

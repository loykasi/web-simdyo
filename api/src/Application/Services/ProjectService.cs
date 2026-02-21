using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Models.Requests;
using Application.Models.Requests.Project;
using Application.Models.Responses.Project;
using Application.Results;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
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
        private const long _thumbnailSizeLimit = 2 * 1024 * 1024;
        private const long _projectSizeLimit = 15 * 1024 * 1024;
        private const int _uploadLimit = 3;

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

        public async Task<Result<UploadProjectResponse>> RequestUpload(UploadProjectRequest request)
        {
            var user = await currentUserService.GetUserAsync();
            if (user == null)
            {
                return Result.NotFound<UploadProjectResponse>
                (
                    new Error("Profile.InvalidUserToken", "Invalid token")
                );
            }

            var date = DateOnly.FromDateTime(DateTime.Now);
            var stat = await unitOfWork.UserDailyUploadStatsRepository
                .GetByUserIdAsync(user.Id, date);

            if (stat == null)
            {
                stat = new UserDailyUploadStats
                {
                    User = user,
                    Date = date,
                    UploadCount = 0
                };
                unitOfWork.UserDailyUploadStatsRepository.Add(stat);
            }

            if (stat.UploadCount >= _uploadLimit)
            {
                return Result.BadRequest<UploadProjectResponse>
                (
                    new Error("Project.LimitReached", "Daily upload limit reached")
                );
            }

            if (request.ThumbnailLength > _thumbnailSizeLimit||
                request.ProjectLength > _projectSizeLimit)
            {
                return Result.BadRequest<UploadProjectResponse>
                (
                    new Error("Project.Validation", "File size exceeds limit")
                );
            }

            ProjectCategory? category = null;
            if (request.Category != null)
            {
                category = await unitOfWork.ProjectCategoryRepository.GetByName(request.Category);
                if (category == null)
                {
                    return Result.NotFound<UploadProjectResponse>
                    (
                        new Error("Category.Invalid", "Invalid category")
                    );
                }
            }

            Project project = new()
            {
                Name = request.Title,
                Description = request.Description,
                Category = category,
                User = user
            };
            unitOfWork.ProjectRepository.Add(project);

            string projectName = $"{project.PublicId}_{GetUniqueHash}.simdyo";
            string thumbnailName = $"{project.PublicId}_{GetUniqueHash}.png";

            project.PublicId = publicIdService.Encode(project.Id);
            project.FileLink = objectStorageService.GetPath(projectName);
            project.ThumbnailLink = objectStorageService.GetPath(thumbnailName);

            bool isGetProjectLinkSuccess = objectStorageService.TryGetPreSignedUrl
            (
                projectName,
                "application/x-simdyo",
                request.ProjectLength,
                out string projectPresignedUrl
            );
            bool isGetThumbnailLinkSuccess = objectStorageService.TryGetPreSignedUrl
            (
                thumbnailName,
                "image/png",
                request.ThumbnailLength,
                out string thumbnailPresignedUrl
            );

            if (!isGetProjectLinkSuccess || !isGetThumbnailLinkSuccess)
            {
                return Result.NotFound<UploadProjectResponse>
                (
                    new Error("Upload.Failure", "Request upload failed")
                );
            }

            stat.UploadCount++;
            await unitOfWork.SaveChangesAsync();

            var uploadResponse = new UploadProjectResponse
            (
                PublicId: project.PublicId,
                ProjectPresignedUrl: projectPresignedUrl,
                ThumbnaiPresignedUrl: thumbnailPresignedUrl
            );

            return Result.Success(uploadResponse);
        }

        public async Task<Result<UploadProjectResponse>> Update(string publicId, UpdateProjectRequest request)
        {
            if (request.ThumbnailLength > _thumbnailSizeLimit ||
                request.ProjectLength > _projectSizeLimit)
            {
                return Result.BadRequest<UploadProjectResponse>
                (
                    new Error("Profile.Validation", "File size exceeds limit")
                );
            }

            string userID = currentUserService.GetUserID();

            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return Result.NotFound<UploadProjectResponse>
                (
                    new Error("Profile.InvalidUserToken", "Invalid token")
                );
            }

            var project = await publicIdService.GetProject(publicId);
            if (project == null)
            {
                return Result.NotFound<UploadProjectResponse>
                (
                    new Error("Project.NotFound", $"No project with id: {publicId}.")
                );
            }

            if (project.UserId != user.Id)
            {
                return Result.NotFound<UploadProjectResponse>
                (
                    new Error("Project.Unauthorized", $"Cannot delete project with id: {publicId}.")
                );
            }
            
            project.Name = request.Title;
            project.Description = request.Description;

            if (request.Category != null)
            {
                var category = await unitOfWork.ProjectCategoryRepository.GetByName(request.Category);
                if (category == null)
                {
                    return Result.NotFound<UploadProjectResponse>
                    (
                        new Error("Category.Invalid", "Invalid category")
                    );
                }

                project.Category = category;
            }

            string projectPresignedUrl = string.Empty;
            string thumbnailPresignedUrl = string.Empty;
            List<string> oldFileNames = [];

            if (request.ProjectLength.HasValue)
            {
                string projectName = $"{project.PublicId}_{GetUniqueHash}.simdyo";
                if (objectStorageService.TryGetPreSignedUrl
                (
                    projectName,
                    "application/x-simdyo",
                    request.ProjectLength.Value,
                    out projectPresignedUrl
                ))
                {
                    oldFileNames.Add(Path.GetFileName(project.FileLink));
                    project.FileLink = objectStorageService.GetPath(projectName);
                }
            }

            if (request.ThumbnailLength.HasValue)
            {
                string thumbnailName = $"{project.PublicId}_{GetUniqueHash}.png";
                if (objectStorageService.TryGetPreSignedUrl
                (
                    thumbnailName,
                    "image/png",
                    request.ThumbnailLength.Value,
                    out thumbnailPresignedUrl
                ))
                {
                    oldFileNames.Add(Path.GetFileName(project.ThumbnailLink));
                    project.ThumbnailLink = objectStorageService.GetPath(thumbnailName);
                }
            }

            var uploadResponse = new UploadProjectResponse
            (
                PublicId: project.PublicId,
                ProjectPresignedUrl: projectPresignedUrl,
                ThumbnaiPresignedUrl: thumbnailPresignedUrl
            );

            await unitOfWork.SaveChangesAsync();

            objectStorageService.DeleteJobs(oldFileNames);

            return Result.Success(uploadResponse);
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

        public async Task<Result<DailyUploadLimitResponse>> GetDailyLimit()
        {
            var user = await currentUserService.GetUserAsync();

            if (user == null)
            {
                return Result.UnAuthorized<DailyUploadLimitResponse>
                (
                    new Error("Auth.Unauthorized", $"Unauthorized")
                );
            }

            var date = DateOnly.FromDateTime(DateTime.Now);
            var dailyLimit = await unitOfWork.UserDailyUploadStatsRepository
                .GetByUserIdAsync(user.Id, date);

            var response = new DailyUploadLimitResponse
            {
                Limit = _uploadLimit,
                UploadCount = dailyLimit != null ? dailyLimit.UploadCount : 0
            };

            return Result.Success(response);
        }

        private static string GetUniqueHash => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString("x");
    }
}

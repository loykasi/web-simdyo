using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Models.Requests;
using Application.Models.Responses;
using Application.Results;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class ProjectReportService
    (
        UserManager<User> userManager,
        ICurrentUserService currentUserService,
        IPublicIdService publicIdService,
        IUnitOfWork unitOfWork
    ) : IProjectReportService
    {
        public async Task<Result<Pagination<ProjectReportDto>>> Get(string? filter, int? page = null, int? limit = null)
        {
            var pagination = await unitOfWork.ProjectReportRepository.Get(filter, page, limit);

            return Result.Success(pagination);
        }

        public async Task<Result> AddReport(string publicId, ReportProjectRequest payload)
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

            ProjectReport report = new()
            {
                Reason = payload.Reason,
                Description = payload.Description,
                Project = project,
                ByUser = user
            };

            unitOfWork.ProjectReportRepository.Add(report);
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

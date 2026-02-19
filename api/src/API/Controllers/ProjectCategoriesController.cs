using Application.Authorization;
using Application.Interfaces.Services;
using Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/projects/categories")]
    public class ProjectCategoriesController(IProjectCategoryService projectCategoryService) : BaseController
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetNames()
        {
            var result = await projectCategoryService.GetNames();
            return ToApiResult(result);
        }

        [HttpGet]
        [RequirePermission(Permissions.DashboardAccess)]
        [RequirePermission(Permissions.ManageCategories)]
        public async Task<IActionResult> Get()
        {
            var result = await projectCategoryService.Get();
            return ToApiResult(result);
        }

        [HttpPost]
        [RequirePermission(Permissions.DashboardAccess)]
        [RequirePermission(Permissions.ManageCategories)]
        public async Task<IActionResult> Add(AddProjectCategoryRequest payload)
        {
            var result = await projectCategoryService.Add(payload);
            return ToApiResult(result);
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.DashboardAccess)]
        [RequirePermission(Permissions.ManageCategories)]
        public async Task<IActionResult> Update(int id, UpdateProjectCategoryRequest payload)
        {
            var result = await projectCategoryService.Update(id, payload);
            return ToApiResult(result);
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.DashboardAccess)]
        [RequirePermission(Permissions.ManageCategories)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await projectCategoryService.Delete(id);
            return ToApiResult(result);
        }
    }
}

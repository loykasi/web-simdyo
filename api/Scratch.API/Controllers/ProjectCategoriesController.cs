using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scratch.Application.Interfaces.Services;
using Scratch.Domain.Requests;

namespace Scratch.API.Controllers
{
    [Route("api/projects/categories")]
    public class ProjectCategoriesController(IProjectCategoryService projectCategoryService) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await projectCategoryService.Get();
            return ToApiResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProjectCategoryRequest payload)
        {
            var result = await projectCategoryService.Add(payload);
            return ToApiResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProjectCategoryRequest payload)
        {
            var result = await projectCategoryService.Update(id, payload);
            return ToApiResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await projectCategoryService.Delete(id);
            return ToApiResult(result);
        }
    }
}

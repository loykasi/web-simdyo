using Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult ToApiResult(Result result)
        {
            if (result.IsSuccess)
            {
                return Ok();
            }

            return ToProblem(result);
        }

        protected IActionResult ToApiResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return ToProblem(result);
        }

        protected IActionResult ToProblem(Result result)
        {
            int statusCode = result.TypeOfError switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                ErrorType.AccessUnAuthorized => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            return new ObjectResult(result.Errors)
            {
                StatusCode = statusCode,
            };
        }
    }
}

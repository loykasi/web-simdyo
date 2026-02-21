using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Handlers
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            //var (statusCode, message) = GetExceptionDetails(exception);
            //httpContext.Response.StatusCode = (int)statusCode;
            //await httpContext.Response.WriteAsJsonAsync(message, cancellationToken);

            _logger.LogError(exception, "Unhandled exception occurred");

            httpContext.Response.StatusCode = exception switch
            {
                ApplicationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            await httpContext.Response.WriteAsJsonAsync
            (
                new ProblemDetails
                {
                    Type = exception.GetType().Name,
                    Title = "An error occured",
                    Detail = exception.Message
                }
            );

            return true;
        }

        private (HttpStatusCode statusCode, string message) GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                UserExistsException => (HttpStatusCode.Conflict, exception.Message),
                LoginFailedException => (HttpStatusCode.Unauthorized, exception.Message),
                RegistrationFailedException => (HttpStatusCode.Unauthorized, exception.Message),
                RefreshTokenException => (HttpStatusCode.Unauthorized, exception.Message),
                ConfirmTokenException => (HttpStatusCode.Unauthorized, exception.Message),
                _ => (HttpStatusCode.InternalServerError, $"An unexpected error occured: {exception.Message}")
            };
        }
    }
}

using Microsoft.AspNetCore.Diagnostics;
using Scratch.Domain.Exceptions;
using System.Net;

namespace Scratch.API.Handlers
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, message) = GetExceptionDetails(exception);

            _logger.LogError(exception, exception.Message);

            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(message, cancellationToken);

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

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Exceptions.Handler
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            (string Details, string Title, int StatusCode) details = exception switch
            {
                InternalServerErrorException=>(
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError),

                BadRequestException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status400BadRequest),

                UnauthorizedAccessException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status401Unauthorized),

                NotFoundException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status404NotFound),

                _ => (
                "An unexpected error occurred.",
                "InternalServerErrorException",
                StatusCodes.Status500InternalServerError)

            };

            var problemDetails = new ProblemDetails
            {
                Title = details.Title,
                Status = details.StatusCode,
                Detail = details.Details,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);


            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions.Add("errors", validationException);
            }

            httpContext.Response.StatusCode = details.StatusCode;

            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

            return true;

        }
    }
}

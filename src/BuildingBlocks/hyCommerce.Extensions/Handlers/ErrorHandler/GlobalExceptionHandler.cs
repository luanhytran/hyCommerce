using hyCommerce.Extensions.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace hyCommerce.Extensions.Handlers.ErrorHandler;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private const string INVALID_ARGUMENT = "invalid_argument";
    private const string NOT_FOUND = "not_found";
    private const string INTERNAL_ERROR = "internal_error";
    public const string CONCURRENCY_ERROR_MESSAGE = "A concurrency error occurred when executing this request. Please retry again.";
    public const string INTERNAL_ERROR_MESSAGE = "An error occurred. Please try again later.";

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = exception switch
        {
            ArgumentNullException ex => CreateProblemDetails(HttpStatusCode.BadRequest, INVALID_ARGUMENT, ex.Message, httpContext),
            ArgumentException ex => CreateProblemDetails(HttpStatusCode.BadRequest, INVALID_ARGUMENT, ex.Message, httpContext),
            ValidationException ex => CreateProblemDetails(HttpStatusCode.BadRequest, INVALID_ARGUMENT, ex.Message, httpContext),
            UnauthorizedAccessException ex => CreateProblemDetails(HttpStatusCode.BadRequest, INVALID_ARGUMENT, ex.Message, httpContext),
            BadRequestException ex => CreateProblemDetails(HttpStatusCode.BadRequest, INVALID_ARGUMENT, ex.Message, httpContext),
            NotFoundException ex => CreateProblemDetails(HttpStatusCode.NotFound, NOT_FOUND, ex.Message, httpContext),
            _ => CreateProblemDetails(HttpStatusCode.InternalServerError, INTERNAL_ERROR, INTERNAL_ERROR_MESSAGE, httpContext)
        };

        httpContext.Response.StatusCode = (int)problemDetails.Status!;

        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

        return true;
    }

    private static ProblemDetails CreateProblemDetails(HttpStatusCode statusCode, string errorCode, string errorMessage, HttpContext httpContext)
    {
        return new ProblemDetails
        {
            Status = (int)statusCode,
            Extensions =
            {
                { "errorCode", errorCode },
                { "errorMessage", errorMessage },
                { "traceId", httpContext.TraceIdentifier }
            }
        };
    }
}
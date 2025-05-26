using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Helpers;

public class ErrorResponseHandler : IErrorResponseHandler
{
    public IActionResult CreateErrorResponse(Error error)
    {
        var problemDetails = new ProblemDetails
        {
            Title = error.Message,
            Status = GetStatusCode(error.Code),
        };

        problemDetails.Extensions["code"] = error.Code;

        if (error.EntityId.HasValue && error.EntityType != null)
        {
            problemDetails.Extensions["entityId"] = error.EntityId;
            problemDetails.Extensions["entityType"] = error.EntityType.Name;
        }

        if (error.ValidationErrors != null)
        {
            problemDetails.Extensions["validationErrors"] = error.ValidationErrors;
        }

        return new ObjectResult(problemDetails);
    }

    private static int GetStatusCode(string errorCode) => errorCode switch
    {
        ErrorCodes.EntityNotFound => StatusCodes.Status404NotFound,
        ErrorCodes.ValidationError => StatusCodes.Status400BadRequest,
        ErrorCodes.Conflict => StatusCodes.Status409Conflict,
        ErrorCodes.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorCodes.Forbidden => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError
    };
}

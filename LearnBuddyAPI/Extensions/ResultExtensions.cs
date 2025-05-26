using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Helpers;

namespace WebApplication.Extensions;

public static class ResultExtensions
{
    #region HTTP 200 OK
    
    public static IActionResult To200ActionResult(this Result result, IErrorResponseHandler? errorHandler = null)
    {
        errorHandler ??= new ErrorResponseHandler();
        return result.IsSuccess ? new OkResult() : errorHandler.CreateErrorResponse(result.Error);
    }
    
    public static IActionResult To200ActionResult<T>(this Result<T> result, IErrorResponseHandler? errorHandler = null)
    {
        errorHandler ??= new ErrorResponseHandler();
        return result.IsSuccess ? new OkObjectResult(result.Value) : errorHandler.CreateErrorResponse(result.Error);
    }
    
    #endregion
    
    #region HTTP 201 Created
    public static IActionResult To201ActionResult<T>(
        this Result<T> result,
        IErrorResponseHandler? errorHandler = null)
    {
        errorHandler ??= new ErrorResponseHandler();

        if (!result.IsSuccess)
            return errorHandler.CreateErrorResponse(result.Error);
        return new StatusCodeResult(StatusCodes.Status201Created);
    }

    public static IActionResult To201ActionResult(
        this Result result,
        IErrorResponseHandler? errorHandler = null)
    {
        errorHandler ??= new ErrorResponseHandler();

        if (!result.IsSuccess)
            return errorHandler.CreateErrorResponse(result.Error);
        return new StatusCodeResult(StatusCodes.Status201Created);
    }
    
    #endregion
    
    #region HTTP 204 No Content
    
    public static IActionResult To204ActionResult(this Result result, IErrorResponseHandler? errorHandler = null)
    {
        errorHandler ??= new ErrorResponseHandler();
        return result.IsSuccess ? new NoContentResult() : errorHandler.CreateErrorResponse(result.Error);
    }
    
    // No generic version needed for 204 since it doesn't return content
    
    #endregion
}

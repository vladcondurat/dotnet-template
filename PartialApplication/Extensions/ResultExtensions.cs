using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Helpers;

namespace WebApplication1.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult(this Result result, ErrorResponseHandler errorHandler)
        {
            return result.IsSuccess ? new NoContentResult() : errorHandler.CreateErrorResponse(result.Error);
        }

        public static IActionResult ToActionResult<T>(this Result<T> result, ErrorResponseHandler errorHandler)
        {
            return result.IsSuccess ? new OkObjectResult(result.Value) : errorHandler.CreateErrorResponse(result.Error);
        }
    }
}
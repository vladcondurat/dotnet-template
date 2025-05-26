using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Helpers
{
    public interface IErrorResponseHandler
    {
        IActionResult CreateErrorResponse(Error error);
    }
}
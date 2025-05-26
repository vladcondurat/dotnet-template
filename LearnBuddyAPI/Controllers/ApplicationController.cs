using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication.Controllers;

[ApiController]
[Authorize]
public class ApplicationController : ControllerBase
{
    protected Guid? UserId
    {
        get
        {
            if (HttpContext.User is null)
            {
                return null;
            }

            var currentUser = HttpContext.User;

            if (!currentUser.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                return null;
            }

            var contextUserId = currentUser.Claims.FirstOrDefault(user => user.Type == ClaimTypes.NameIdentifier)!.Value;
            var isParsed = Guid.TryParse(contextUserId, out var userId);

            return isParsed ? userId : null;
        }
    }

    protected void ValidateUserId()
    {
        if (UserId is null)
        {
            throw new ArgumentNullException(nameof(UserId), "User Id not found");
        }
    }
}

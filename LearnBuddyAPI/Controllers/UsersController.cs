using Application.UseCases.Queries.UserQueries;
using Application.UseCases.Commands.UserCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Controllers;

[Route("/api/v1/[controller]")]
[ApiController]
public class UsersController : ApplicationController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        ValidateUserId();
        var result = await _mediator.Send(new GetCurrentUserQuery { UserId = UserId!.Value });
        return result.To200ActionResult();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return result.To200ActionResult();
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
        return result.To200ActionResult();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _mediator.Send(new DeleteUserByIdCommand { Id = id });
        return result.To204ActionResult();
    }
}
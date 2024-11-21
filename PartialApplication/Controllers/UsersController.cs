using Application.Use_Classes.Commands.UserCommands;
using Application.Use_Classes.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Extensions;
using WebApplication1.Helpers;

namespace WebApplication1.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ErrorResponseHandler _errorHandler;

    public UsersController(IMediator mediator, ErrorResponseHandler errorHandler)
    {
        _mediator = mediator;
        _errorHandler = errorHandler;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToActionResult(_errorHandler);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return result.ToActionResult(_errorHandler);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
        return result.ToActionResult(_errorHandler);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(new { message = "Mismatched UserQueries ID" });
        }

        var result = await _mediator.Send(command);
        return result.ToActionResult(_errorHandler);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _mediator.Send(new DeleteUserByIdCommand { Id = id });
        return result.ToActionResult(_errorHandler);
    }
}
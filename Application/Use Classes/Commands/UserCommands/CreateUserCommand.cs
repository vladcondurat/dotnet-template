using Domain.Common;
using MediatR;

namespace Application.Use_Classes.Commands.UserCommands;

public class CreateUserCommand : IRequest<Result<Guid>>
{
    public string Username { get; set; } = string.Empty;
}
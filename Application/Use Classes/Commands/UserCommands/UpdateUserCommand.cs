using Domain.Common;
using MediatR;

namespace Application.Use_Classes.Commands.UserCommands;

public class UpdateUserCommand : IRequest<Result>
{   
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
}
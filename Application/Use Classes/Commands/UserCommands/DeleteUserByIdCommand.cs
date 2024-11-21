using Domain.Common;
using MediatR;

namespace Application.Use_Classes.Commands.UserCommands;

public class DeleteUserByIdCommand : IRequest<Result>
{
   public Guid Id { get; set; }
}
using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.UserCommands;

public class DeleteUserByIdCommand : IRequest<Result>
{
   public Guid Id { get; set; }
}
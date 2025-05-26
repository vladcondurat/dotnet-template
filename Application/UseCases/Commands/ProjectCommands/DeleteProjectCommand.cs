using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.ProjectCommands;

public class DeleteProjectCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
} 
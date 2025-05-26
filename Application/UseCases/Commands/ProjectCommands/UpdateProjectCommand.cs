using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.ProjectCommands;

public class UpdateProjectCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
} 
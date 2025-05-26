using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.ProjectCommands;

public class CreateProjectCommand : IRequest<Result<Guid>>
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Size { get; set; }
} 
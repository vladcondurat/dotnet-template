using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.ProjectCommands;

public class UpdateProjectSummaryCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Summary { get; set; } = string.Empty;
}
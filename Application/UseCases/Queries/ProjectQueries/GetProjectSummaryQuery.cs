using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.ProjectQueries;

public class GetProjectSummaryQuery : IRequest<Result<ProjectContentDto>>
{
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
}
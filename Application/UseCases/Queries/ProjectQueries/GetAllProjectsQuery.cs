using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.ProjectQueries;

public class GetAllProjectsQuery : IRequest<Result<IEnumerable<ProjectDto>>>
{
    public Guid UserId { get; set; }
} 
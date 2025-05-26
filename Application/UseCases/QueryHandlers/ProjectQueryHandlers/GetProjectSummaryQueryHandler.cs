using Application.DTOs;
using Application.UseCases.Queries.ProjectQueries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.ProjectQueryHandlers;

public class
    GetProjectSummaryQueryHandler : IRequestHandler<GetProjectSummaryQuery, Result<ProjectContentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProjectSummaryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<ProjectContentDto>> Handle(GetProjectSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.FindAsync(request.ProjectId);
        if (project == null)
        {
            return Error.EntityNotFound(request.ProjectId, typeof(Project));
        }
        if (project.UserId != request.UserId)
        {
            return Error.Forbidden("You do not have permission to access this project");
        }

        var projectContentDto = _mapper.Map<ProjectContentDto>(project);
        
        return Result<ProjectContentDto>.Success(projectContentDto);
    }
}
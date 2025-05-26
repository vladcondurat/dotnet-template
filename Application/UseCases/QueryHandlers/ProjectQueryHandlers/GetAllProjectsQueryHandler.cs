using Application.DTOs;
using Application.UseCases.Queries.ProjectQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.ProjectQueryHandlers;

public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, Result<IEnumerable<ProjectDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllProjectsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _unitOfWork.Projects.GetProjectsByUserIdAsync(request.UserId);
        var projectDtos = _mapper.Map<IEnumerable<ProjectDto>>(projects);
        
        return Result<IEnumerable<ProjectDto>>.Success(projectDtos);
    }
} 
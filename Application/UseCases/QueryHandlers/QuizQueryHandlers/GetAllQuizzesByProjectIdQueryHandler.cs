using Application.DTOs;
using Application.UseCases.Queries.QuizQueries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.QuizQueryHandlers;

public class GetAllQuizzesByProjectIdQueryHandler : IRequestHandler<GetAllQuizzesByProjectIdQuery, Result<IEnumerable<QuizDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllQuizzesByProjectIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<QuizDto>>> Handle(GetAllQuizzesByProjectIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.FindAsync(request.ProjectId);
        if (project == null)
        {
            return Error.EntityNotFound(request.ProjectId, typeof(Project));
        }
        
        if (project.UserId != request.UserId)
        {
            return Error.Forbidden("You do not have permission to access this project.");
        }

        var quizzes = await _unitOfWork.Quizzes.GetQuizzesByProjectIdAsync(request.ProjectId);
        
        var quizDtos = _mapper.Map<IEnumerable<QuizDto>>(quizzes);
        return Result<IEnumerable<QuizDto>>.Success(quizDtos);
    }
} 
using Application.DTOs;
using Application.UseCases.Queries.QuizQueries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.QueryHandlers.QuizQueryHandlers;

public class GetQuizByIdQueryHandler : IRequestHandler<GetQuizByIdQuery, Result<QuizDetailDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetQuizByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<QuizDetailDto>> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
    {
        var quiz = await _unitOfWork.Quizzes.GetQuizWithQuestionsAndProjectAsync(request.QuizId);
        if (quiz == null)
        {
            return Error.EntityNotFound(request.QuizId, typeof(Quiz));
        }
        
        if (quiz.Project.UserId != request.UserId)
        {
            return Error.Forbidden("You do not have permission to access this quiz.");
        }

        var quizDetailDto = _mapper.Map<QuizDetailDto>(quiz);
        return Result<QuizDetailDto>.Success(quizDetailDto);
    }
} 
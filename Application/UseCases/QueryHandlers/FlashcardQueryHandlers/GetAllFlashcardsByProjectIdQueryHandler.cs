using Application.DTOs;
using Application.UseCases.Queries.FlashcardQueries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.FlashcardQueryHandlers;

public class GetAllFlashcardsByProjectIdQueryHandler : IRequestHandler<GetAllFlashcardsByProjectIdQuery, Result<FlashcardsResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllFlashcardsByProjectIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<FlashcardsResponse>> Handle(GetAllFlashcardsByProjectIdQuery request, CancellationToken cancellationToken)
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

        var flashcards = (await _unitOfWork.Flashcards
                .GetFlashcardsByProjectIdAsync(request.ProjectId))
            .ToList();

        var flashcardDtos = _mapper.Map<List<FlashcardDto>>(flashcards);

        var response = new FlashcardsResponse
        {
            Flashcards = flashcardDtos,
            NumberOfFlashcards = flashcardDtos.Count
        };

        return Result<FlashcardsResponse>.Success(response);
    }
} 
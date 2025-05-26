using Application.Services;
using Application.UseCases.Commands.FlashcardCommands;
using Domain.Common;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.FlashcardCommandHandlers;

public class GenerateFlashcardsCommandHandler : IRequestHandler<GenerateFlashcardsCommand, Result<IEnumerable<Guid>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGeminiService _geminiService;

    public GenerateFlashcardsCommandHandler(
        IUnitOfWork unitOfWork,
        IGeminiService geminiService)
    {
        _unitOfWork = unitOfWork;
        _geminiService = geminiService;
    }

    public async Task<Result<IEnumerable<Guid>>> Handle(GenerateFlashcardsCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.FindAsync(request.ProjectId);
        if (project is null)
        {
            return Error.EntityNotFound(request.ProjectId, typeof(Project));
        }
        
        if (project.UserId != request.UserId)
        {
            return Error.Forbidden("You do not have permission to access this project.");
        }

        var generatedData = await _geminiService.GenerateFlashcardsAsync(
            project.Summary,
            request.NumberOfFlashcards,
            request.Difficulty,
            request.FocusArea);

        var flashcards = generatedData
            .Select(data => new Flashcard
            {
                FlashcardId = Guid.NewGuid(),
                Question = data.Question,
                Answer = data.Answer,
                IsKnown = false,
                ProjectId = request.ProjectId
            })
            .ToList();

        await _unitOfWork.Flashcards.AddRangeAsync(flashcards);
        await _unitOfWork.SaveChangesAsync();

        var flashcardIds = flashcards.Select(fc => fc.FlashcardId);
        return Result<IEnumerable<Guid>>.Success(flashcardIds);
    }
}

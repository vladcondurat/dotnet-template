using Application.Services;
using Application.UseCases.Commands.FlashcardCommands;
using Domain.Common;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.FlashcardCommandHandlers;

public class ToggleFlashcardKnownCommandHandler : IRequestHandler<ToggleFlashcardKnownCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public ToggleFlashcardKnownCommandHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(ToggleFlashcardKnownCommand request, CancellationToken cancellationToken)
    {
        var flashcard = await _unitOfWork.Flashcards.GetFlashcardByIdIncludeProjectAsync(request.FlashcardId);
        if (flashcard is null)
        {
            return Error.EntityNotFound(request.FlashcardId, typeof(Flashcard));
        }

        if (flashcard.Project.UserId != request.UserId)
        {
            return Error.Forbidden("You do not have permission to access this flashcard.");
        }

        flashcard.IsKnown = !flashcard.IsKnown;
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
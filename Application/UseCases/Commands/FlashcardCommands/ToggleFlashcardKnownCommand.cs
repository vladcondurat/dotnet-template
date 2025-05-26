using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.FlashcardCommands;

public class ToggleFlashcardKnownCommand : IRequest<Result>
{
    public Guid FlashcardId { get; set; }
    public Guid UserId { get; set; }
}
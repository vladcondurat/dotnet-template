using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.FlashcardCommands;

public class GenerateFlashcardsCommand : IRequest<Result<IEnumerable<Guid>>>
{
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public int NumberOfFlashcards { get; set; }
    public string? FocusArea { get; set; }
} 
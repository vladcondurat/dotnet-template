using Application.UseCases.Commands.FlashcardCommands;
using FluentValidation;

namespace Application.Validators.Flashcard;

public class GenerateFlashcardsCommandValidator : AbstractValidator<GenerateFlashcardsCommand>
{
    public GenerateFlashcardsCommandValidator()
    {
        RuleFor(command => command.ProjectId)
            .NotEmpty().WithMessage("Project ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("Project ID must not be empty.");
        
        RuleFor(command => command.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("User ID must not be empty.");

        RuleFor(command => command.Difficulty)
            .NotEmpty().WithMessage("Difficulty is required.")
            .Must(d => new[] { "Easy", "Medium", "Hard" }.Contains(d))
            .WithMessage("Difficulty must be one of: Easy, Medium, Hard.");

        RuleFor(command => command.NumberOfFlashcards)
            .NotEmpty().WithMessage("Number of flashcards is required.")
            .InclusiveBetween(1, 50).WithMessage("Number of flashcards must be between 1 and 50.");

        When(command => !string.IsNullOrEmpty(command.FocusArea), () => {
            RuleFor(command => command.FocusArea)
                .MaximumLength(100).WithMessage("Focus area cannot exceed 100 characters.");
        });
    }
} 
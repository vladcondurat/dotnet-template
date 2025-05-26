using Application.UseCases.Commands.QuizCommands;
using FluentValidation;

namespace Application.Validators.Quiz;

public class GenerateQuizCommandValidator : AbstractValidator<GenerateQuizCommand>
{
    public GenerateQuizCommandValidator()
    {
        RuleFor(command => command.ProjectId)
            .NotEmpty().WithMessage("Project ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("Project ID must not be empty.");
        
        RuleFor(command => command.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("User ID must not be empty.");

        RuleFor(command => command.Topic)
            .NotEmpty().WithMessage("Topic is required.")
            .MaximumLength(100).WithMessage("Topic cannot exceed 100 characters.");

        RuleFor(command => command.Difficulty)
            .NotEmpty().WithMessage("Difficulty is required.")
            .Must(d => new[] { "Easy", "Medium", "Hard" }.Contains(d))
            .WithMessage("Difficulty must be one of: Easy, Medium, Hard.");

        RuleFor(command => command.NumberOfQuestions)
            .NotEmpty().WithMessage("Number of questions is required.")
            .InclusiveBetween(1, 20).WithMessage("Number of questions must be between 1 and 20.");
    }
} 
using Application.UseCases.Commands.ProjectCommands;
using FluentValidation;

namespace Application.Validators.Project;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(command => command.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(command => command.Content)
            .NotEmpty().WithMessage("Content is required.");

        RuleFor(command => command.Size)
            .NotEmpty().WithMessage("Size is required.");
    }
} 
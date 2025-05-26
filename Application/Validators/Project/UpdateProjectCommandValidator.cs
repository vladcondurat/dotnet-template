using Application.UseCases.Commands.ProjectCommands;
using FluentValidation;

namespace Application.Validators.Project;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("Project ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("Project ID must not be empty.");
        
        RuleFor(command => command.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("User ID must not be empty.");

        RuleFor(command => command.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
        RuleFor(command => command.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
} 
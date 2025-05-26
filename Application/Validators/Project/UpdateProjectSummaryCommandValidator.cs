using Application.UseCases.Commands.ProjectCommands;
using FluentValidation;

namespace Application.Validators.Project;

public class UpdateProjectSummaryCommandValidator : AbstractValidator<UpdateProjectSummaryCommand>
{
    public UpdateProjectSummaryCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("Project ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("Project ID must not be empty.");
        
        RuleFor(command => command.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("User ID must not be empty.");

        RuleFor(command => command.Summary)
            .NotEmpty().WithMessage("Summary is required.");
    }
} 
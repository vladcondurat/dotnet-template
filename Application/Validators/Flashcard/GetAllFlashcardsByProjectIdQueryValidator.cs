using Application.UseCases.Queries.FlashcardQueries;
using FluentValidation;

namespace Application.Validators.Flashcard;

public class GetAllFlashcardsByProjectIdQueryValidator : AbstractValidator<GetAllFlashcardsByProjectIdQuery>
{
    public GetAllFlashcardsByProjectIdQueryValidator()
    {
        RuleFor(query => query.ProjectId)
            .NotEmpty().WithMessage("Project ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("Project ID must not be empty.");
        
        RuleFor(query => query.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("User ID must not be empty.");
    }
} 
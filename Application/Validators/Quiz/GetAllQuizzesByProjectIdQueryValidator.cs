using Application.UseCases.Queries.QuizQueries;
using FluentValidation;

namespace Application.Validators.Quiz;

public class GetAllQuizzesByProjectIdQueryValidator : AbstractValidator<GetAllQuizzesByProjectIdQuery>
{
    public GetAllQuizzesByProjectIdQueryValidator()
    {
        RuleFor(query => query.ProjectId)
            .NotEmpty().WithMessage("Project ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("Project ID must not be empty.");
        RuleFor(query => query.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("User ID must not be empty.");
    }
} 
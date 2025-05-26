using Application.UseCases.Queries.QuizQueries;
using FluentValidation;

namespace Application.Validators.Quiz;

public class GetQuizByIdQueryValidator : AbstractValidator<GetQuizByIdQuery>
{
    public GetQuizByIdQueryValidator()
    {
        RuleFor(query => query.QuizId)
            .NotEmpty().WithMessage("Quiz ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("Quiz ID must not be empty.");
        RuleFor(query => query.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("User ID must not be empty.");
    }
} 
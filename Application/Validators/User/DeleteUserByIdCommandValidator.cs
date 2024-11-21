using Application.Use_Classes.Commands.UserCommands;
using FluentValidation;

namespace Application.Validators.User;

public class DeleteUserByIdCommandValidator: AbstractValidator<DeleteUserByIdCommand>
{
    public DeleteUserByIdCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("UserQueries ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("UserQueries ID must not be empty.");
    }
}
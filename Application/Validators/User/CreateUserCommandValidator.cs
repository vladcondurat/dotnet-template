using Application.Use_Classes.Commands.UserCommands;
using Domain.Repositories;
using FluentValidation;

namespace Application.Validators.User
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters")
                .MustAsync(BeUniqueUsername).WithMessage("Username must be unique");
            
        }

        private async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Users.IsUsernameUniqueAsync(username);
        }
    }
}

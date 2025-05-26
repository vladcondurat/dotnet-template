using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.AuthCommands;

public class ForgotPasswordCommand : IRequest<Result>
{
    public string Email { get; set; } = string.Empty;
} 
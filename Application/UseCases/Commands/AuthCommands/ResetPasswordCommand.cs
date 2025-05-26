using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.AuthCommands;

public class ResetPasswordCommand : IRequest<Result>
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
} 
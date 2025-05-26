using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.AuthCommands;

public class ConfirmEmailCommand : IRequest<Result>
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
} 
using Application.DTOs;
using Application.DTOs.Auth;
using Application.DTOs.Auth.Responses;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.AuthCommands;

public class LoginCommand : IRequest<Result<LoginResponseDto>>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
} 
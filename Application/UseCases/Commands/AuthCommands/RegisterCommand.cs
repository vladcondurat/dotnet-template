using Application.DTOs;
using Application.DTOs.Auth;
using Application.DTOs.Auth.Responses;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.AuthCommands;

public class RegisterCommand : IRequest<Result>
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
} 
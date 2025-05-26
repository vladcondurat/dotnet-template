using Application.DTOs;
using Application.DTOs.Auth;
using Application.DTOs.Auth.Responses;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.AuthCommands;

public class RefreshTokenCommand : IRequest<Result<RefreshTokenResponseDto>>
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
} 
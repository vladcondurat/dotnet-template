using Application.DTOs.Auth.Requests;
using Application.UseCases.Commands.AuthCommands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Extensions;

namespace WebApplication.Controllers;

[Route("/api/v1/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController : ApplicationController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var command = _mapper.Map<LoginCommand>(request);
        var result = await _mediator.Send(command);
        return result.To200ActionResult();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        var result = await _mediator.Send(command);
        return result.To201ActionResult();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var command = _mapper.Map<RefreshTokenCommand>(request);
        var result = await _mediator.Send(command);
        return result.To200ActionResult();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
    {
        var command = _mapper.Map<ForgotPasswordCommand>(request);
        var result = await _mediator.Send(command);
        return result.To204ActionResult();
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var command = new ConfirmEmailCommand
        {
            UserId = userId,
            Token = token
        };
        
        var result = await _mediator.Send(command);
        return result.To204ActionResult();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        var command = _mapper.Map<ResetPasswordCommand>(request);
        var result = await _mediator.Send(command);
        return result.To204ActionResult();
    }
} 
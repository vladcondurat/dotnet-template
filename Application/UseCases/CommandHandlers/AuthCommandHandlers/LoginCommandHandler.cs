using Application.DTOs;
using Application.DTOs.Auth;
using Application.DTOs.Auth.Responses;
using Application.UseCases.Commands.AuthCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.CommandHandlers.AuthCommandHandlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;

    public LoginCommandHandler(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenService jwtTokenService,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
    }

    public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return Error.Unauthorized("Invalid username or password");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return Error.Unauthorized("Invalid username or password");
        }

        if (!user.EmailConfirmed)
        {
            return Error.ValidationError("Please confirm your email address before logging in");
        }

        var accessToken = await _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        
        // Store refresh token with longer expiry if "Remember Me" is checked
        var refreshTokenExpiryDays = request.RememberMe ? 30 : 7;
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);
        user.UpdatedAt = DateTime.UtcNow;
        
        await _userManager.UpdateAsync(user);

        var userDto = _mapper.Map<UserDto>(user);
        var roles = await _userManager.GetRolesAsync(user);
        userDto.Roles = roles.ToList();

        var authResponse = new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expiration = _jwtTokenService.GetTokenExpiryDate(),
            User = userDto,
            RequiresEmailConfirmation = false
        };

        return Result<LoginResponseDto>.Success(authResponse);
    }
} 
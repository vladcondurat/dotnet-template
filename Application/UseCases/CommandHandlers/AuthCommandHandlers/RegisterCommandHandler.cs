using Application.DTOs.Auth.Responses;
using Application.UseCases.Commands.AuthCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Application.DTOs;
using Domain.Exceptions;

namespace Application.UseCases.CommandHandlers.AuthCommandHandlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(
            UserManager<User> userManager,
            IJwtTokenService jwtTokenService,
            IEmailService emailService,
            IMapper mapper)
        {
            _userManager     = userManager;
            _jwtTokenService = jwtTokenService;
            _emailService    = emailService;
            _mapper          = mapper;
        }

        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ConfirmPassword)
                return Error.ValidationError("Password and confirmation password do not match");

            var clientAppBaseUrl = Environment.GetEnvironmentVariable("CLIENT_BASE_URL");
            var verifyEndpoint   = Environment.GetEnvironmentVariable("CLIENT_VERIFY_EMAIL_ENDPOINT");
            if (string.IsNullOrWhiteSpace(clientAppBaseUrl) || string.IsNullOrWhiteSpace(verifyEndpoint))
            {
                return Error.ValidationError("Client callback configuration is missing");
            }

            var baseUrl  = clientAppBaseUrl.TrimEnd('/');
            var endpoint = verifyEndpoint.TrimStart('/');

            if (await _userManager.FindByNameAsync(request.UserName) != null)
            {
                return Error.ValidationError("Username is already taken");
                
            }

            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return Error.ValidationError("Email is already registered");
                
            }

            var user = new User
            {
                UserName       = request.UserName,
                Email          = request.Email,
                CreatedAt      = DateTime.UtcNow,
                UpdatedAt      = DateTime.UtcNow,
                EmailConfirmed = false
            };
            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                return Error.ValidationError(createResult.Errors.Select(e => e.Description));
                
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return Error.ValidationError(roleResult.Errors.Select(e => e.Description));
            }

            var token       = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedUser = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Id.ToString()));
            var encodedTok  = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmLink = $"{baseUrl}/{endpoint}?userId={encodedUser}&token={encodedTok}";

            var emailResult = await _emailService.SendEmailConfirmationAsync(user.Email!, confirmLink);
            if (emailResult.IsFailure)
            {
                await _userManager.DeleteAsync(user);
                return emailResult;
            }

            var accessToken  = await _jwtTokenService.GenerateAccessToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            user.RefreshToken           = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            user.UpdatedAt              = DateTime.UtcNow;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return Error.ValidationError(updateResult.Errors.Select(e => e.Description));
            }

            var authResponse = new LoginResponseDto
            {
                AccessToken               = accessToken,
                RefreshToken              = refreshToken,
                Expiration                = DateTime.UtcNow.AddMinutes(15),
                User                      = _mapper.Map<UserDto>(user),
                RequiresEmailConfirmation = true
            };
            return Result<LoginResponseDto>.Success(authResponse);
        }
    }
}

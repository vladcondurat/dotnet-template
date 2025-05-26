using Application.UseCases.Commands.AuthCommands;
using Domain.Common;
using Domain.Entities;
using Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Application.DTOs.Auth.Responses;
using Domain.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace Application.UseCases.CommandHandlers.AuthCommandHandlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponseDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenCommandHandler(
            UserManager<User> userManager,
            IJwtTokenService jwtTokenService,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<Result<RefreshTokenResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal principal;
            try
            {
                principal = _jwtTokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            }
            catch (SecurityTokenException)
            {
                return Error.Unauthorized("Invalid access token");
            }
            catch (Exception ex)
            {
                return Error.ServerError($"Token validation error: {ex.Message}");
            }

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Error.Unauthorized("Invalid token");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null
                || user.RefreshToken != request.RefreshToken
                || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Error.Unauthorized("Invalid or expired refresh token");
            }

            if (!user.EmailConfirmed)
                return Error.ValidationError("Email not confirmed");

            var newAccessToken = await _jwtTokenService.GenerateAccessToken(user);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var dto = new RefreshTokenResponseDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Expiration = _jwtTokenService.GetTokenExpiryDate(),
            };

            return Result<RefreshTokenResponseDto>.Success(dto);
        }
    }
}

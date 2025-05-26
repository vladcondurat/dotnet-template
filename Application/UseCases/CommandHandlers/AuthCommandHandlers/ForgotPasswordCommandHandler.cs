using Application.UseCases.Commands.AuthCommands;
using Domain.Common;
using Domain.Entities;
using Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Domain.Exceptions;

namespace Application.UseCases.CommandHandlers.AuthCommandHandlers
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(
            UserManager<User> userManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Result.Success();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var encodedUserId = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Id.ToString()));

            var clientAppBaseUrl = Environment.GetEnvironmentVariable("CLIENT_BASE_URL");
            var resetEndpoint = Environment.GetEnvironmentVariable("CLIENT_RESET_PASSWORD_ENDPOINT");

            if (string.IsNullOrWhiteSpace(clientAppBaseUrl) || string.IsNullOrWhiteSpace(resetEndpoint))
            {
                return Error.ValidationError("Environment variable missing.");
            }

            var baseUrl = clientAppBaseUrl.TrimEnd('/');
            var endpoint = resetEndpoint.TrimStart('/');
            var resetLink = $"{baseUrl}/{endpoint}?userId={encodedUserId}&token={encodedToken}";

            var emailResult = await _emailService.SendPasswordResetAsync(user.Email!, resetLink);
            if (emailResult.IsFailure)
            {
                return emailResult;
            }

            return Result.Success();
        }
    }
}
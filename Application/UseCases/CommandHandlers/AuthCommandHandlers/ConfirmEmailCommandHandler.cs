using Application.UseCases.Commands.AuthCommands;
using Domain.Common;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Application.UseCases.CommandHandlers.AuthCommandHandlers;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result>
{
    private readonly UserManager<User> _userManager;

    public ConfirmEmailCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var decodedUserId = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.UserId));
            var userId = Guid.Parse(decodedUserId);
            
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return Error.EntityNotFound(userId, typeof(User));
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
            {
                return Error.ValidationError("Email confirmation failed. The confirmation link may be invalid or expired.");
            }

            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Error.ServerError($"Email confirmation failed: {ex.Message}");
        }
    }
} 
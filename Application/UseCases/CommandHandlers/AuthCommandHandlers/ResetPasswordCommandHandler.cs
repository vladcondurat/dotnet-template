using Application.UseCases.Commands.AuthCommands;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Domain.Exceptions;

namespace Application.UseCases.CommandHandlers.AuthCommandHandlers;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result> {
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.NewPassword != request.ConfirmPassword)
            return Error.ValidationError("Password and confirmation password do not match");

        Guid userId;
        try
        {
            var decoded = WebEncoders.Base64UrlDecode(request.UserId);
            var decodedText = Encoding.UTF8.GetString(decoded);
            userId = Guid.Parse(decodedText);
        }
        catch (FormatException)
        {
            return Error.ValidationError("Invalid reset link");
        }
        catch (ArgumentException)
        {
            return Error.ValidationError("Invalid reset link");
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Error.EntityNotFound(userId, typeof(User));

        string decodedToken;
        try
        {
            var tokenBytes = WebEncoders.Base64UrlDecode(request.Token);
            decodedToken = Encoding.UTF8.GetString(tokenBytes);
        }
        catch (FormatException)
        {
            return Error.ValidationError("Invalid or expired token");
        }

        var resetResult = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
        if (!resetResult.Succeeded)
            return Error.ValidationError(resetResult.Errors.Select(e => e.Description));

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        user.UpdatedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return Result.Success();
    }
}

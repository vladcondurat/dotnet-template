using Domain.Common;
using Domain.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class DevelopmentEmailService : IEmailService
{
    private readonly ILogger<DevelopmentEmailService> _logger;

    public DevelopmentEmailService(ILogger<DevelopmentEmailService> logger)
    {
        _logger = logger;
    }

    public Task<Result> SendEmailAsync(string email, string subject, string message)
    {
        _logger.LogInformation("---------- DEVELOPMENT EMAIL ----------");
        _logger.LogInformation($"To: {email}");
        _logger.LogInformation($"Subject: {subject}");
        _logger.LogInformation($"Body: {message}");
        _logger.LogInformation("--------------------------------------");

        return Task.FromResult(Result.Success());
    }

    public Task<Result> SendEmailConfirmationAsync(string email, string confirmationLink)
    {
        _logger.LogInformation("---------- EMAIL CONFIRMATION LINK ----------");
        _logger.LogInformation($"To: {email}");
        _logger.LogInformation($"Confirmation Link: {confirmationLink}");
        _logger.LogInformation("--------------------------------------------");

        var subject = "Confirm your Learn Buddy account";
        var username = email.Split('@')[0];
        var message = EmailTemplates.GetEmailConfirmationTemplate(confirmationLink, username);
        
        return Task.FromResult(Result.Success());
    }

    public Task<Result> SendPasswordResetAsync(string email, string resetLink)
    {
        _logger.LogInformation("---------- PASSWORD RESET LINK ----------");
        _logger.LogInformation($"To: {email}");
        _logger.LogInformation($"Reset Link: {resetLink}");
        _logger.LogInformation("----------------------------------------");

        var subject = "Reset your Learn Buddy password";
        var username = email.Split('@')[0];
        var message = EmailTemplates.GetPasswordResetTemplate(resetLink, username);
        
        return Task.FromResult(Result.Success());
    }
} 
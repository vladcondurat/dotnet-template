using Domain.Common;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task<Result> SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            var client = new SendGridClient(_emailSettings.SendGridApiKey);
            var from = new EmailAddress(_emailSettings.FromEmail, _emailSettings.FromName);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, message);
            
            var response = await client.SendEmailAsync(msg);
            
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                _logger.LogError($"Failed to send email. Status Code: {response.StatusCode}, Body: {responseBody}");
                return Error.ExternalServiceError($"Failed to send email via SendGrid. Status Code: {response.StatusCode}");
            }
            
            _logger.LogInformation($"Email sent successfully to {email}");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending email to {email}");
            return Error.ServerError($"Error sending email: {ex.Message}");
        }
    }

    public async Task<Result> SendEmailConfirmationAsync(string email, string confirmationLink)
    {
        var subject = "Confirm your Learn Buddy account";
        // Extract username from email or use first part of email
        var username = email.Split('@')[0];
        var message = EmailTemplates.GetEmailConfirmationTemplate(confirmationLink, username);
        return await SendEmailAsync(email, subject, message);
    }

    public async Task<Result> SendPasswordResetAsync(string email, string resetLink)
    {
        var subject = "Reset your Learn Buddy password";
        // Extract username from email or use first part of email
        var username = email.Split('@')[0];
        var message = EmailTemplates.GetPasswordResetTemplate(resetLink, username);
        return await SendEmailAsync(email, subject, message);
    }
} 
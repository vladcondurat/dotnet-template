using Domain.Common;

namespace Domain.Services;

public interface IEmailService
{
    Task<Result> SendEmailAsync(string email, string subject, string message);
    Task<Result> SendEmailConfirmationAsync(string email, string confirmationLink);
    Task<Result> SendPasswordResetAsync(string email, string resetLink);
} 
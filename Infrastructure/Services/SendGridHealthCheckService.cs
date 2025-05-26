using Domain.Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using SendGrid;

namespace Infrastructure.Services;

public class SendGridHealthCheckService : IHealthCheck
{
    private readonly EmailSettings _emailSettings;

    public SendGridHealthCheckService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var client = new SendGridClient(_emailSettings.SendGridApiKey);
            
            var response = await client.RequestAsync(
                method: BaseClient.Method.GET,
                urlPath: "/");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return HealthCheckResult.Healthy("SendGrid API key is valid.");
            }
            else
            {
                return HealthCheckResult.Unhealthy("SendGrid API key is invalid.");
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Error checking SendGrid health.", ex);
        }
    }
} 
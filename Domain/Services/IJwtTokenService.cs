using System.Security.Claims;
using Domain.Entities;

namespace Domain.Services;

public interface IJwtTokenService
{
    Task<string> GenerateAccessToken(User user);
    string GenerateRefreshToken();
    DateTime GetTokenExpiryDate();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
} 
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<Guid>
{
    // IdentityUser provides:
    // - Id (Guid)
    // - UserName
    // - Email
    // - PhoneNumber
    // - PasswordHash
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    
    public IEnumerable<Project> Projects { get; set; } = new List<Project>();
}
namespace Application.DTOs.Auth.Requests;

public class LoginRequestDto
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
} 
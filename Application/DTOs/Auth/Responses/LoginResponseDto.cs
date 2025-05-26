namespace Application.DTOs.Auth.Responses;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public UserDto User { get; set; } = new UserDto();
    public bool RequiresEmailConfirmation { get; set; }
}
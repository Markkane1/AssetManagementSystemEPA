using AssetManagement.Application.DTOs.Auth;

namespace AssetManagement.Blazor.Models;

public class AuthSession
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserInfoDto? User { get; set; }
}

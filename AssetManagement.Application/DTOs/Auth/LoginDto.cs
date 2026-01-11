namespace AssetManagement.Application.DTOs.Auth;

public record LoginDto
{
    public string Username { get; set; } = String.Empty;
    public string Password { get; set; }=String.Empty;
}

namespace AssetManagement.Application.DTOs.Auth;

public record AuthResponseDto(
    string Token,
    string RefreshToken,
    DateTime ExpiresAt,
    UserInfoDto User
);

public record UserInfoDto(
    string Id,
    string Username,
    string Email,
    string FirstName,
    string LastName,
    List<string> Permissions
);

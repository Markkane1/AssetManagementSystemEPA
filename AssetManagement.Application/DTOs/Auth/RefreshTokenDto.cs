namespace AssetManagement.Application.DTOs.Auth;

public record RefreshTokenDto(
    int Id,
    string Token,
    string UserId,
    DateTime CreatedAt,
    DateTime ExpiresAt,
    bool IsRevoked,
    bool IsUsed,
    string? DeviceInfo,
    string? IPAddress
);

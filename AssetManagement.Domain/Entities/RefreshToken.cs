namespace AssetManagement.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public bool IsUsed { get; set; }
    // New fields for device and IP tracking
    public string? DeviceInfo { get; set; }
    public string? IPAddress { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsUsed && !IsExpired;

    public RefreshToken() { }

    // Existing constructor retained for backward compatibility
    public RefreshToken(string token, string userId, int expiryDays = 7)
    {
        Token = token;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddDays(expiryDays);
    }

    // New constructor that captures device information
    public RefreshToken(string token, string userId, string? deviceInfo, string? ipAddress, int expiryDays = 7)
    {
        Token = token;
        UserId = userId;
        DeviceInfo = deviceInfo;
        IPAddress = ipAddress;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddDays(expiryDays);
    }

    public void Revoke() => IsRevoked = true;
    public void Use() => IsUsed = true;
    // Optional: record revocation time if needed
    public DateTime? RevokedAt { get; set; }
}

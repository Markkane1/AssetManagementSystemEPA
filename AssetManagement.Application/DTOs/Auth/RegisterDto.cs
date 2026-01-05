namespace AssetManagement.Application.DTOs.Auth;

public record RegisterDto(
    string Email,
    string Username,
    string Password,
    string FirstName,
    string LastName
);

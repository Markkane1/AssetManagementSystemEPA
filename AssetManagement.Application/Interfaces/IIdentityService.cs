using AssetManagement.Application.DTOs.Auth;

namespace AssetManagement.Application.Interfaces;

public interface IIdentityService
{
    Task<(bool Succeeded, string UserId, string[] Errors)> RegisterAsync(RegisterDto registerDto);
    Task<(bool Succeeded, string UserId, string[] Errors)> CheckPasswordAsync(string username, string password);
    Task<UserInfoDto?> GetUserByIdAsync(string userId);
    Task<UserInfoDto?> GetUserByNameAsync(string username);
    Task<List<int>> GetAllowedLocationIdsAsync(string userId);
    Task<List<string>> GetRolesAsync();
    Task<bool> AddToRoleAsync(string userId, string roleName);
    Task<bool> RemoveFromRoleAsync(string userId, string roleName);
    Task<IList<string>> GetUserRolesAsync(string userId);
    Task<List<string>> GetUserPermissionsAsync(string userId);
}

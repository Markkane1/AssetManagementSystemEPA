using System.Net.Http.Json;
using AssetManagement.Application.DTOs.Auth;

namespace AssetManagement.Blazor.Services;

public class RoleService
{
    private readonly HttpClient _httpClient;

    public RoleService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<string>> GetRolesAsync()
        => await _httpClient.GetFromJsonAsync<List<string>>("api/roles") ?? [];

    public async Task<List<RolePermissionDto>> GetRolePermissionsAsync(string roleId)
        => await _httpClient.GetFromJsonAsync<List<RolePermissionDto>>($"api/roles/{roleId}/permissions") ?? [];

    public async Task<bool> AssignPermissionAsync(string roleId, int permissionId)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/roles/{roleId}/permissions",
            new AssignRolePermissionRequest(permissionId));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RevokePermissionAsync(string roleId, int permissionId)
    {
        var response = await _httpClient.DeleteAsync($"api/roles/{roleId}/permissions/{permissionId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AddUserToRoleAsync(string userId, string roleName)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/roles/users/{userId}/roles", roleName);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveUserFromRoleAsync(string userId, string roleName)
    {
        var response = await _httpClient.DeleteAsync($"api/roles/users/{userId}/roles/{roleName}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<string>> GetUserRolesAsync(string userId)
        => await _httpClient.GetFromJsonAsync<List<string>>($"api/roles/users/{userId}/roles") ?? [];

    private record AssignRolePermissionRequest(int PermissionId);
}

public class PermissionCatalogService
{
    private readonly HttpClient _httpClient;

    public PermissionCatalogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PermissionDto>> GetAllAsync()
        => await _httpClient.GetFromJsonAsync<List<PermissionDto>>("api/permissions") ?? [];

    public async Task<List<string>> GetCategoriesAsync()
        => await _httpClient.GetFromJsonAsync<List<string>>("api/permissions/categories") ?? [];

    public async Task<List<PermissionDto>> GetByCategoryAsync(string category)
        => await _httpClient.GetFromJsonAsync<List<PermissionDto>>($"api/permissions/category/{category}") ?? [];
}

public class UserAccessService
{
    private readonly HttpClient _httpClient;

    public UserAccessService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserLocationAccessDto>> GetUserLocationsAsync(string userId)
        => await _httpClient.GetFromJsonAsync<List<UserLocationAccessDto>>($"api/users/{userId}/access/locations") ?? [];

    public async Task<bool> SetUserLocationsAsync(string userId, List<int> locationIds)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/users/{userId}/access/locations", locationIds);
        return response.IsSuccessStatusCode;
    }
}

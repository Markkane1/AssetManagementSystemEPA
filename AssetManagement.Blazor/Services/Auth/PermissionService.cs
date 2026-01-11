using AssetManagement.Blazor.Models;
using Blazored.LocalStorage;

namespace AssetManagement.Blazor.Services.Auth;

public class PermissionService
{
    private const string StorageKey = "auth-session";
    private readonly ILocalStorageService _localStorage;

    public PermissionService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<bool> HasPermissionAsync(string permission)
    {
        var session = await _localStorage.GetItemAsync<AuthSession>(StorageKey);
        return session?.User?.Permissions.Contains(permission) ?? false;
    }
}

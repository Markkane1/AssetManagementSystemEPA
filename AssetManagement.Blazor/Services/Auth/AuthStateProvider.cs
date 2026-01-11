using System.Security.Claims;
using AssetManagement.Blazor.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace AssetManagement.Blazor.Services.Auth;

public class AuthStateProvider : AuthenticationStateProvider
{
    private const string StorageKey = "auth-session";
    private readonly ILocalStorageService _localStorage;

    public AuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var session = await _localStorage.GetItemAsync<AuthSession>(StorageKey);
        if (session?.User == null || string.IsNullOrWhiteSpace(session.Token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, session.User.Id),
            new(ClaimTypes.Name, session.User.Username),
            new(ClaimTypes.Email, session.User.Email)
        };

        claims.AddRange(session.User.Permissions.Select(permission => new Claim("permission", permission)));

        var identity = new ClaimsIdentity(claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task NotifyUserAuthenticationAsync(AuthSession session)
    {
        await _localStorage.SetItemAsync(StorageKey, session);
        var authState = await GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public async Task NotifyUserLogoutAsync()
    {
        await _localStorage.RemoveItemAsync(StorageKey);
        var anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        NotifyAuthenticationStateChanged(Task.FromResult(anonymous));
    }

    // Add this protected method to expose the base class method
    protected void NotifyAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        base.NotifyAuthenticationStateChanged(task);
    }
}

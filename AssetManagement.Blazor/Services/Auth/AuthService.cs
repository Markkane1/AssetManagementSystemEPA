using System.Net.Http.Json;
using AssetManagement.Application.DTOs.Auth;
using AssetManagement.Blazor.Models;
using Blazored.LocalStorage;

namespace AssetManagement.Blazor.Services.Auth;

public class AuthService
{
    private const string StorageKey = "auth-session";
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthStateProvider _authStateProvider;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<AuthSession?> GetSessionAsync()
    {
        return await _localStorage.GetItemAsync<AuthSession>(StorageKey);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        if (authResponse == null)
        {
            return null;
        }

        await StoreSessionAsync(authResponse);
        return authResponse;
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        if (authResponse == null)
        {
            return null;
        }

        await StoreSessionAsync(authResponse);
        return authResponse;
    }

    public async Task<AuthResponseDto?> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/refresh-token",
            new RefreshTokenRequest(accessToken, refreshToken));
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        if (authResponse == null)
        {
            return null;
        }

        await StoreSessionAsync(authResponse);
        return authResponse;
    }

    public async Task<UserInfoDto?> GetCurrentUserAsync()
    {
        var response = await _httpClient.GetAsync("api/auth/me");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<UserInfoDto>();
    }

    public async Task SignOutAsync()
    {
        await _authStateProvider.NotifyUserLogoutAsync();
    }

    private async Task StoreSessionAsync(AuthResponseDto authResponse)
    {
        var session = new AuthSession
        {
            Token = authResponse.Token,
            RefreshToken = authResponse.RefreshToken,
            ExpiresAt = authResponse.ExpiresAt,
            User = authResponse.User
        };

        await _authStateProvider.NotifyUserAuthenticationAsync(session);
    }

    private record RefreshTokenRequest(string AccessToken, string RefreshToken);
}

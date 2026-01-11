using System.Net;
using System.Net.Http.Headers;
using AssetManagement.Blazor.Models;
using Blazored.LocalStorage;

namespace AssetManagement.Blazor.Services.Auth;

public class ApiAuthMessageHandler : DelegatingHandler
{
    private const string StorageKey = "auth-session";
    private readonly ILocalStorageService _localStorage;

    public ApiAuthMessageHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var session = await _localStorage.GetItemAsync<AuthSession>(StorageKey);
        if (session != null && !string.IsNullOrWhiteSpace(session.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", session.Token);
        }

        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException)
        {
            return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            {
                RequestMessage = request,
                ReasonPhrase = "API unreachable"
            };
        }
    }
}

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly ProtectedSessionStorage _sessionStorage;

    public AuthorizationMessageHandler(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {        
        var result = await _sessionStorage.GetAsync<string>("authToken");
        string token = result.Success ? result.Value : null;

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
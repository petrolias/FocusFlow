using FocusFlow.Blazor;

public class AuthorizationMessageHandler(IHttpContextAccessor accessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var cookie = accessor.HttpContext?.Request.Cookies[Constants.CookieAccessToken];
        if (!string.IsNullOrEmpty(cookie)) {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cookie);
        }        

        return await base.SendAsync(request, cancellationToken);
    }
}
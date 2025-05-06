using FocusFlow.Abstractions.Extensions;
using FocusFlow.Blazor.Models;
using FocusFlow.Blazor;

public class AuthorizationMessageHandler(IHttpContextAccessor accessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var cookie = accessor.HttpContext?.Request.Cookies[Constants.CookieAccessToken];
        if (cookie != null) {
            var data = cookie.FromJson<CookieModel>();
            if (data != null && !string.IsNullOrEmpty(data.Token))
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", data.Token);
        }        

        return await base.SendAsync(request, cancellationToken);
    }
}
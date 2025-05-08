namespace FocusFlow.Blazor.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient LocalApi(this IHttpClientFactory self) => self.CreateClient(Constants.HttpClients.LocalApi);
        public static HttpClient ExternalApi(this IHttpClientFactory self) => self.CreateClient(Constants.HttpClients.ExternalApi);
    }
}

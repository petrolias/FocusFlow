using System.Net.Http.Headers;

namespace FocusFlow.Blazor.Common
{
    public class TokenHttpClient(IHttpContextAccessor accessor, IHttpClientFactory factory) : ITokenHttpClient
    {
        public async Task<HttpResponseMessage> GetAsync(string endpoint) => await GetClient().GetAsync(endpoint);
        public async Task<HttpResponseMessage> PostAsync(string endpoint, object data) => await GetClient().PostAsJsonAsync(endpoint, data);
        public async Task<HttpResponseMessage> PutAsync(string endpoint, object data) => await GetClient().PutAsJsonAsync(endpoint, data);
        public async Task<HttpResponseMessage> DeleteAsync(string endpoint) => await GetClient().DeleteAsync(endpoint);

        public async Task<T?> ReadFromJsonAsync<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<T>();
        }

        private HttpClient GetClient()
        {
            var client = factory.CreateClient("ExternalApi");
            var token = accessor.HttpContext?.Request.Cookies[Constants.CookieAccessToken];
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}

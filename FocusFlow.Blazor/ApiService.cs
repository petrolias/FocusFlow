using System.Net.Http.Headers;

namespace FocusFlow.Blazor
{
    public class ApiService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiService(IHttpContextAccessor accessor, IHttpClientFactory factory)
        {
            _httpContextAccessor = accessor;
            _httpClientFactory = factory;
        }

        public async Task<T?> GetWithTokenAsync<T>(string endpoint)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];
            if (string.IsNullOrEmpty(token)) return default;

            var client = _httpClientFactory.CreateClient("ExternalApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return default;

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}

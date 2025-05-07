using FocusFlow.Abstractions.Api.Shared;

namespace FocusFlow.Blazor.Extensions
{
    public static class LocalApiExtensions
    {        
        public static async Task<HttpResponseMessage> GetTokenAsync(this IHttpClientFactory self, LoginDto model) =>
            await self.LocalApi().PostAsJsonAsync($"/api/auth/token", model);

        public static async Task<HttpResponseMessage> AddUserAsync(this IHttpClientFactory self, RegisterDto model) =>
            await self.LocalApi().PostAsJsonAsync($"/api/users", model);
    }
}
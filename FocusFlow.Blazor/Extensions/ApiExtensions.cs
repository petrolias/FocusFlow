using System.Net.Http.Json;
using System.Reflection;
using FocusFlow.Abstractions.Api.Shared;

namespace FocusFlow.Blazor.Extensions
{
    public static class ApiExtensions
    {
        #region LOCAL_API

        public static async Task<HttpResponseMessage> GetTokenAsync(this IHttpClientFactory self, LoginDto model) =>
            await self.LocalApi().PostAsJsonAsync($"/api/auth/token", model);
     
        public static async Task<HttpResponseMessage> AddUserAsync(this IHttpClientFactory self, RegisterDto model) =>
            await self.LocalApi().PostAsJsonAsync($"/api/users", model);

        #endregion LOCAL_API

        #region EXTERNAL_API

        public static async Task<List<ProjectDto>> GetProjectsAsync(this IHttpClientFactory self) =>
            await self.ExternalApi().GetFromJsonAsync<List<ProjectDto>>("api/projects");

        public static async Task<HttpResponseMessage> AddProjectAsync(this IHttpClientFactory self, ProjectDtoBase model) =>
            await self.ExternalApi().PostAsJsonAsync("api/projects", model);

        public static async Task<HttpResponseMessage> UpdateProjectsAsync(this IHttpClientFactory self, Guid id, ProjectDto model) =>
            await self.ExternalApi().PutAsJsonAsync($"api/projects/{id}", model);

        public static async Task<HttpResponseMessage> DeleteProjectsAsync(this IHttpClientFactory self, Guid id) =>
            await self.ExternalApi().DeleteAsync($"api/projects/{id}");

        public static async Task<HttpResponseMessage> GetExternalTokenAsync(this IHttpClientFactory self, LoginDto model) =>
           await self.ExternalApi().PostAsJsonAsync($"/api/auth/token", model);

        public static async Task<HttpResponseMessage> ExternalAddUserAsync(this IHttpClientFactory self, RegisterDto model) =>
          await self.ExternalApi().PostAsJsonAsync($"/api/users", model);

        #endregion EXTERNAL_API
    }
}
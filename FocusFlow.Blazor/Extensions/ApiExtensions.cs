using FocusFlow.Abstractions.Api.Shared;

namespace FocusFlow.Blazor.Extensions
{
    public static class ApiExtensions
    {        
        public static async Task<List<ProjectDto>> GetProjectsAsync(this IHttpClientFactory self) =>
            await self.ExternalApi().GetFromJsonAsync<List<ProjectDto>>("api/projects");

        public static async Task<HttpResponseMessage> AddProjectAsync(this IHttpClientFactory self, ProjectDtoBase model) =>
            await self.ExternalApi().PostAsJsonAsync("api/projects", model);

        public static async Task<HttpResponseMessage> UpdateProjectsAsync(this IHttpClientFactory self, Guid id, ProjectDto model) =>
            await self.ExternalApi().PutAsJsonAsync($"api/projects/{id}", model);

        public static async Task<HttpResponseMessage> DeleteProjectsAsync(this IHttpClientFactory self, Guid id) =>
            await self.ExternalApi().DeleteAsync($"api/projects/{id}");
    }
}

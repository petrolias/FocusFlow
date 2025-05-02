using Newtonsoft.Json;
using System.Text;
using TStartup = FocusFlow.WebApi.Program;

namespace FocusFlow.WebApi.Tests.Controllers
{
    public class ProjectsControllerTests : IClassFixture<WebApiFactory<FocusFlow.WebApi.Program>>
    {
        private readonly HttpClient _client;

        public ProjectsControllerTests(WebApiFactory<TStartup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProjects_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/projects");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateProject_ReturnsCreated()
        {
            // Arrange
            var newProject = new { Name = "Test Project", Description = "Test Description" };
            var content = new StringContent(JsonConvert.SerializeObject(newProject), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/projects", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpdateProject_ReturnsNoContent()
        {
            // Arrange
            var updatedProject = new { Name = "Updated Project", Description = "Updated Description" };
            var content = new StringContent(JsonConvert.SerializeObject(updatedProject), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/projects/1", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteProject_ReturnsNoContent()
        {
            // Act
            var response = await _client.DeleteAsync("/api/projects/1");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProject_AndGet_ReturnsUpdatedProject()
        {
            // Arrange
            var updatedProject = new { Name = "Updated Project", Description = "Updated Description" };
            var content = new StringContent(JsonConvert.SerializeObject(updatedProject), Encoding.UTF8, "application/json");

            // Act
            var updateResponse = await _client.PutAsync("/api/projects/1", content);

            // Assert Update
            Assert.Equal(System.Net.HttpStatusCode.NoContent, updateResponse.StatusCode);

            // Act Get
            var getResponse = await _client.GetAsync("/api/projects/1");
            getResponse.EnsureSuccessStatusCode();
            var responseData = await getResponse.Content.ReadAsStringAsync();

            // Assert Get
            Assert.Contains("Updated Project", responseData);
            Assert.Contains("Updated Description", responseData);
        }
    }
}
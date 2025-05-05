
namespace FocusFlow.Blazor.Common
{
    public interface ITokenHttpClient
    {
        Task<HttpResponseMessage> DeleteAsync(string endpoint);
        Task<HttpResponseMessage> GetAsync(string endpoint);
        Task<HttpResponseMessage> PostAsync(string endpoint, object data);
        Task<HttpResponseMessage> PutAsync(string endpoint, object data);
        Task<T?> ReadFromJsonAsync<T>(HttpResponseMessage response);
    }
}
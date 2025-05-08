namespace FocusFlow.Blazor.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<string> ReadErrorMessageAsync(this HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                return !string.IsNullOrWhiteSpace(content) ? content : response.ReasonPhrase ?? "Unknown error";
            }
            catch
            {
                return response.ReasonPhrase ?? "Unknown error";
            }
        }
    }
}

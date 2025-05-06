using System.Text.Json;

namespace FocusFlow.Abstractions.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj, bool indented = false)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = indented,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(obj, options);
        }

        public static T? FromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}

using System.Text.Json;

namespace Core
{
    public class Serializer
    {
        public static string Serialize<T>(T obj, JsonSerializerOptions options = null)
        {
            if (options == null)
                options = new JsonSerializerOptions
                {
                };
            return JsonSerializer.Serialize<T>(obj, options);
        }
        public static T Deserialize<T>(string json, JsonSerializerOptions options = null)
        {
            if (options == null)
                options = new JsonSerializerOptions
                {
                };
            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}

#nullable enable
using Newtonsoft.Json;

namespace SiriusXM
{
    public static class ExtensionMethods
    {
        public static string? ToJson(this object? obj)
        {
            return obj == null ? default : JsonConvert.SerializeObject(obj);
        }

        public static T? FromJson<T>(this string json)
        {
            return string.IsNullOrEmpty(json) ? default : JsonConvert.DeserializeObject<T>(json);
        }
    }
}
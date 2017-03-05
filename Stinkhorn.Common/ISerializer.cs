using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Text;

namespace Stinkhorn.Common
{
    public interface ISerializer : IDisposable
    {
        byte[] Serialize<T>(T instance);

        T Deserialize<T>(byte[] bytes);
    }

    public class JsonSerializer : ISerializer
    {
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public JsonSerializerSettings Settings { get; set; } = new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            Converters = { new StringEnumConverter() },
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.None
        };

        public T Deserialize<T>(byte[] bytes)
        {
            var json = Encoding.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        public byte[] Serialize<T>(T instance)
        {
            var json = JsonConvert.SerializeObject(instance, Settings);
            return Encoding.GetBytes(json);
        }

        public void Dispose()
        {
        }
    }
}
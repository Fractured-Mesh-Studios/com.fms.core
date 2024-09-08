using CoreEngine.Interfaces;
using Newtonsoft.Json;

namespace CoreEngine.Data.Serializer
{
    public class JsonSerializer : ISerialization
    {
        private JsonSerializerSettings m_settings;

        public JsonSerializer()
        {
            m_settings = new JsonSerializerSettings();
            m_settings.Formatting = Formatting.Indented;
            m_settings.NullValueHandling = NullValueHandling.Include;
            m_settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, m_settings);
        }

        public string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, m_settings);
        }
    }
}

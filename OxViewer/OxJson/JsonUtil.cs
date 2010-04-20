using Newtonsoft.Json;

namespace OxJson
{
    public static partial class JsonUtil
    {
        public static T Deserialize<T>(string message)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(message);
            }
            catch
            {
                return default(T);
            }
        }

        public static string SerializeMessage(JsonType type, object obj)
        {
            string value = Serialize(obj);
            return Serialize(new JsonMessage(type.ToString().ToLower(), value));
        }

        public static string Serialize(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch
            {
                return null;
            }
        }
    }
}

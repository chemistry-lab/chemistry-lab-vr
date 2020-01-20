using Newtonsoft.Json;

namespace DialogFlow.Utilities
{
    public static class Json
    {
        public static string ToJson(object source)
        {
            return JsonConvert.SerializeObject(source);
        }

        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

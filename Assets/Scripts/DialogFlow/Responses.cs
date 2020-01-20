using System;

using Newtonsoft.Json;

namespace DialogFlow
{
    [Serializable]
    public class AuthResponseBody
    {
        [JsonProperty("access_token")]
        public string accessToken;
        [JsonProperty("token_type")]
        public string tokenType;
        [JsonProperty("expires_in")]
        public int expiresIn;
    }

    [Serializable]
    public class ResponseBody
    {
        public string responseId;
        public QueryResult queryResult;
        public string outputAudio;
    }

    [Serializable]
    public class QueryResult
    {
        public string queryText;
        public string languageCode;
        public string action;
        public string fulfillmentText;
        public float intentDetectionConfidence;
        public float speechRecognitionConfidence;
    }
}

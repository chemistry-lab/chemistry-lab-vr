using System.Collections;
using System.Text;
using System;

using UnityEngine.Networking;
using UnityEngine;

using DialogFlow.Utilities;
using Avatars.Enum;

namespace DialogFlow
{
    public class DialogFlowService
    {
        private string projectId = null;
        private string sessionId = null;

        public DialogFlowService()
        {
            projectId = "chemistry-lab-vr";
            sessionId = Guid.NewGuid().ToString();
        }

        public IEnumerator DetectIntent(AudioClip audio, AvatarGender gender, Action<AudioClip> callback)
        {
            string url = $"https://dialogflow.googleapis.com/v2/projects/{projectId}/agent/sessions/{sessionId}:detectIntent";
            string inputAudio = Convert.ToBase64String(Audio.FromAudioClip(audio));
            if (inputAudio == null)
            {
                callback(null);
                yield break;
            }

            UnityWebRequest request = new UnityWebRequest(url, "POST");
            RequestBody requestBody = GetRequestBody(inputAudio, GetSSMLGender(gender));
            string jsonBody = Json.ToJson(requestBody);
            byte[] rawBody = Encoding.UTF8.GetBytes(jsonBody);

            string accessToken = "";
            yield return DialogFlowAuth.GetToken((string token) => accessToken = token);

            request.SetRequestHeader("Authorization", "Bearer " + accessToken);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawBody);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogWarning(request.error);
                callback(null);
                yield break;
            }

            byte[] rawResponse = request.downloadHandler.data;
            ResponseBody response = GetResponseBody(rawResponse);
            AudioClip outputAudio = response.outputAudio != null
              ? Audio.ToAudioClip(Convert.FromBase64String(response.outputAudio))
              : Audio.EmptyClip();

            callback(outputAudio);
        }

        private string GetSSMLGender(AvatarGender gender)
        {
            if (gender == AvatarGender.Male) return SSMLGender.Male;
            if (gender == AvatarGender.Female) return SSMLGender.Female;
            if (gender == AvatarGender.Neutral) return SSMLGender.Neutral;

            return SSMLGender.Unspecified;
        }

        private RequestBody GetRequestBody(string inputAudio, string gender)
        {
            InputAudioConfig inputAudioConfig = new InputAudioConfig();
            inputAudioConfig.audioEncoding = InputAudioEncoding.Linear16;
            inputAudioConfig.sampleRateHertz = 16000;
            inputAudioConfig.languageCode = "nl-NL";

            VoiceConfig voiceConfig = new VoiceConfig();
            voiceConfig.ssmlGender = gender;

            SynthesizeSpeechConfig synthesizeSpeechConfig = new SynthesizeSpeechConfig();
            synthesizeSpeechConfig.voice = voiceConfig;

            OutputAudioConfig outputAudioConfig = new OutputAudioConfig();
            outputAudioConfig.audioEncoding = OutputAudioEncoding.Linear16;
            outputAudioConfig.synthesizeSpeechConfig = synthesizeSpeechConfig;

            QueryInput queryInput = new QueryInput();
            queryInput.audioConfig = inputAudioConfig;

            RequestBody requestBody = new RequestBody();
            requestBody.outputAudioConfig = outputAudioConfig;
            requestBody.queryInput = queryInput;
            requestBody.inputAudio = inputAudio;

            return requestBody;
        }

        private ResponseBody GetResponseBody(byte[] response)
        {
            string json = Encoding.UTF8.GetString(response);
            return Json.FromJson<ResponseBody>(json);
        }
    }
}

using System.Collections;
using System.Text;
using System;

using UnityEngine.Networking;
using UnityEngine;

using DialogFlow.Utilities;

namespace DialogFlow
{

    public static class DialogFlowAuth
    {
        private static DateTime expires = DateTime.MinValue;
        private static string token = null;

        public static IEnumerator GetToken(Action<string> callback)
        {
            if (expires > DateTime.Now)
            {
                callback(token);
                yield break;
            }

            string jwt = Jwt.GetJwt(Constants.Email, Constants.Keyfile, Constants.Scope);

            WWWForm form = new WWWForm();
            form.AddField("grant_type", Constants.Grant);
            form.AddField("assertion", jwt);
            UnityWebRequest request = UnityWebRequest.Post(Constants.Url, form);

            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogWarning(request.error);
                callback(null);
                yield break;
            }

            byte[] rawResponse = request.downloadHandler.data;
            string jsonResponse = Encoding.UTF8.GetString(rawResponse);
            AuthResponseBody response = Json.FromJson<AuthResponseBody>(jsonResponse);

            expires = DateTime.Now.AddSeconds(response.expiresIn);
            token = response.accessToken;

            callback(response.accessToken);
        }
    }
}

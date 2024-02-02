using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Devenant
{
    public static class Request
    {
        [System.Serializable]
        public class Response
        {
            public readonly bool success;
            public readonly string message;
            public readonly string data;
        }

        public static void Get(string uri, Action<Response> callback = null)
        {
            Get(uri, string.Empty, callback);
        }

        public static void Get(string uri, string token, Action<Response> callback = null)
        {
            SendRequest(UnityWebRequest.Get(uri), token, callback);
        }

        public static void Post(string uri, Dictionary<string, string> formFields, Action<Response> callback = null)
        {
            Post(uri, formFields, string.Empty, callback);  
        }

        public static void Post(string uri, Dictionary<string, string> formFields, string token, Action<Response> callback = null)
        {
            SendRequest(UnityWebRequest.Post(uri, formFields), token, callback);
        }

        private static void SendRequest(UnityWebRequest unityWebRequest, string token, Action<Response> callback)
        {
            if(!string.IsNullOrEmpty(token))
                unityWebRequest.SetRequestHeader("Authorization", "Bearer " + token);

            unityWebRequest.SetRequestHeader("Accept", "application/json");

            unityWebRequest.SendWebRequest().completed += (AsyncOperation asyncOperation) =>
            {
                if(asyncOperation.isDone)
                {
                    SendResponse(unityWebRequest, callback);
                }
            };
        }

        private static void SendResponse(UnityWebRequest uwr, Action<Response> callback)
        {
            long responseCode = uwr.responseCode;

            string log = string.Format("Uri: {0}\nData: {1}\nError: {2}", uwr.uri, uwr.downloadHandler.text, uwr.error);

            Debug.Log("ConnectionManager.cs/ReceiveResponse => \n" + log);

            callback?.Invoke(JsonUtility.FromJson<Response>(uwr.downloadHandler.text));
        }
    }
}
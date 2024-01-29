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
            public readonly string data;
            public readonly string message;
        }

        public static void Get(string uri, Action<Response> callback)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(uri);

            uwr.SetRequestHeader("Accept", "application/json");

            uwr.SendWebRequest().completed += (AsyncOperation asyncOperation) =>
            {
                if(asyncOperation.isDone)
                {
                    SendResponse(uwr, callback);
                }
            };
        }

        public static void Post(string uri, Dictionary<string, string> formFields, Action<Response> callback = null)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(uri, formFields);

            uwr.SetRequestHeader("Accept", "application/json");

            uwr.SendWebRequest().completed += (AsyncOperation asyncOperation) =>
            {
                if(asyncOperation.isDone)
                {
                    SendResponse(uwr, callback);
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
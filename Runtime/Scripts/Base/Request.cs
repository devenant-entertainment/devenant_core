using System;
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

            public Response(bool success, string message, string data)
            {
                this.success = success;
                this.message = message;
                this.data = data;
            }
        }

        public static void Get(string uri, Action<Response> callback = null)
        {
            SendRequest(UnityWebRequest.Get(uri), callback);
        }

        public static void Post(string uri, Dictionary<string, string> formFields, Action<Response> callback = null)
        {
            SendRequest(UnityWebRequest.Post(uri, formFields), callback);
        }

        private static void SendRequest(UnityWebRequest unityWebRequest, Action<Response> callback)
        {
            unityWebRequest.SetRequestHeader("Accept", "application/json");

            unityWebRequest.SendWebRequest().completed += (AsyncOperation asyncOperation) =>
            {
                if(asyncOperation.isDone)
                {
                    Debug.Log("Request/SendRequest => \n" + string.Format("Uri: {0}\nData: {1}\nError: {2}", unityWebRequest.uri, unityWebRequest.downloadHandler.text, unityWebRequest.error));

                    callback?.Invoke(JsonUtility.FromJson<Response>(unityWebRequest.downloadHandler.text));
                }
            };
        }
    }
}
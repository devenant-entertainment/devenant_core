using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class DataManager : Singleton<DataManager>
    {
        [System.Serializable]
        public class GameList
        {
            public Game[] games;
        }

        [System.Serializable]
        public class Game
        {
            public string id;
            public string name;
            public string date;
        }

        [SerializeField] private string remoteCall;

        public void List(Action<GameList> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.data.token }
            };

            Request.Post(remoteCall + "get", formFields, (Request.Response response) => 
            { 
                if(response.success)
                {
                    callback?.Invoke(JsonUtility.FromJson<GameList>(response.data));
                }
                else
                {
                    callback?.Invoke(null);
                }
            });
        }

        public void Create<T>(string name, T data, Action<bool> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.data.token },
                { "name", name },
                { "data", JsonUtility.ToJson(data) }
            };

            Request.Post(remoteCall + "create", formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    callback?.Invoke(true);
                }
                else
                {
                    callback?.Invoke(false);
                }
            });
        }

        public void Delete(Game game, Action<bool> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.data.token },
                { "id", game.id },
                { "name", game.name }
            };

            Request.Post(remoteCall + "delete", formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    callback?.Invoke(true);
                }
                else
                {
                    callback?.Invoke(false);
                }
            });
        }

        public void Load<T>(Game game, Action<T> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.data.token },
                { "id", game.id },
                { "name", game.name }
            };

            Request.Post(remoteCall + "load", formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    callback?.Invoke(JsonUtility.FromJson<T>(response.data));
                }
                else
                {
                    callback?.Invoke(default);
                }
            });
        }

        public void Save<T>(string id, string name, T data, Action<bool> callback = null)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.data.token },
                { "id", id },
                { "name", name },
                { "data", JsonUtility.ToJson(data) }
            };

            Request.Post(remoteCall + "save", formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    callback?.Invoke(true);
                }
                else
                {
                    callback?.Invoke(false);
                }
            });
        }
    }
}

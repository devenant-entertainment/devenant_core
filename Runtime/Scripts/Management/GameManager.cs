using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class GameManager : Singleton<GameManager>
    {
        public Game[] games { get { return _games; } private set { _games = value; } }
        private Game[] _games;

        public void Setup(Action<bool> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.user.token }
            };

            Request.Post(ApplicationManager.instance.backend.gameGet, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    GameResponse data = JsonUtility.FromJson<GameResponse>(response.data);

                    games = new Game[data.games.Length];

                    for(int i = 0; i < games.Length; i++)
                    {
                        games[i] = new Game(data.games[i]);
                    }

                    callback?.Invoke(true);
                }
                else
                {
                    callback?.Invoke(false);
                }
            });
        }

        public void Delete(string name, Action<bool> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.user.token },
                { "name", name }
            };

            Request.Post(ApplicationManager.instance.backend.gameDelete, formFields, (Request.Response response) =>
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

        public void Load<T>(string name, Action<T> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.user.token },
                { "name", name }
            };

            Request.Post(ApplicationManager.instance.backend.gameLoad, formFields, (Request.Response response) =>
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

        public void Save<T>(string name, T data, Action<bool> callback = null)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.user.token },
                { "name", name },
                { "data", JsonUtility.ToJson(data) }
            };

            Request.Post(ApplicationManager.instance.backend.gameSave, formFields, (Request.Response response) =>
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

using System;
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
            Request.Get(ApplicationManager.instance.backend.gameGet, UserManager.instance.user.token, (Request.Response response) => 
            { 
                if(response.success)
                {
                    GameResponse data = JsonUtility.FromJson<GameResponse>(response.data);

                    games = new Game[data.games.Length];

                    for(int i = 0; i < games.Length; i++)
                    {
                        games[i] = new Game(data.games[i].id, data.games[i].name, DateTime.Parse(data.games[i].date));
                    }

                    callback?.Invoke(true);
                }
                else
                {
                    callback?.Invoke(false);
                }
            });
        }

        public void Create<T>(string name, T data, Action<bool> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "name", name },
                { "data", JsonUtility.ToJson(data) }
            };

            Request.Post(ApplicationManager.instance.backend.gameCreate, formFields, UserManager.instance.user.token, (Request.Response response) =>
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
                { "id", game.id },
                { "name", game.name }
            };

            Request.Post(ApplicationManager.instance.backend.gameDelete, formFields, UserManager.instance.user.token, (Request.Response response) =>
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
                { "id", game.id },
                { "name", game.name }
            };

            Request.Post(ApplicationManager.instance.backend.gameLoad, formFields, UserManager.instance.user.token, (Request.Response response) =>
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
                { "id", id },
                { "name", name },
                { "data", JsonUtility.ToJson(data) }
            };

            Request.Post(ApplicationManager.instance.backend.gameSave, formFields, UserManager.instance.user.token, (Request.Response response) =>
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

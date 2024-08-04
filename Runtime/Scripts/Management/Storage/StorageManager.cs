using System;
using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class StorageManager : Singleton<StorageManager>, IInitializable
    {
        public Storage[] storages { get { return _storages.ToArray(); } }
        private List<Storage> _storages;

        public void Initialize(Action<InitializationResponse> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.data.token }
            };

            Request.Post(BackendManager.instance.data.storageGet, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    StorageResponse storageResponse = JsonUtility.FromJson<StorageResponse>(response.data);

                    _storages = new List<Storage>();

                    foreach(StorageResponse.Storage storage in storageResponse.storages)
                    {
                        _storages.Add(new Storage(storage));
                    }

                    callback?.Invoke(new InitializationResponse(true));
                }
                else
                {
                    callback?.Invoke(new InitializationResponse(false));
                }
            });
        }

        public Storage Get<T>(string name)
        {
            foreach(Storage storage in _storages)
            {
                if(storage.name == name && storage.type == typeof(T).Name)
                {
                    return storage;
                }
            }

            return null;
        }

        public Storage[] Get<T>()
        {
            List<Storage> storages = new List<Storage>();

            foreach(Storage storage in _storages)
            {
                if(storage.type == typeof(T).Name)
                {
                    storages.Add(storage);
                }
            }

            return storages.ToArray();
        }

        public bool Has<T>(string name)
        {
            return Get<T>(name) != null;
        }

        public void Delete<T>(string name, Action<bool> callback)
        {
            if(Has<T>(name))
            {
                Dictionary<string, string> formFields = new Dictionary<string, string>
                {
                    { "token", UserManager.instance.data.token },
                    { "name", name },
                    { "type", typeof(T).Name }
                };

                Request.Post(BackendManager.instance.data.storageDelete, formFields, (Request.Response response) =>
                {
                    if(response.success)
                    {
                        _storages.Remove(Get<T>(name));

                        callback?.Invoke(true);
                    }
                    else
                    {
                        callback?.Invoke(false);
                    }
                });
            }
            else
            {
                callback?.Invoke(false);
            }
        }

        public void Load<T>(string name, Action<T> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.data.token },
                { "name", name },
                { "type", typeof(T).Name }
            };

            Request.Post(BackendManager.instance.data.storageLoad, formFields, (Request.Response response) =>
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
                { "token", UserManager.instance.data.token },
                { "name", name },
                { "type", typeof(T).Name },
                { "data", JsonUtility.ToJson(data) }
            };

            Request.Post(BackendManager.instance.data.storageSave, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    if (_storages.Find((x)=>x.name == name && x.type == typeof(T).Name) == null)
                    {
                        _storages.Add(new Storage(name, typeof(T).Name));
                    }

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

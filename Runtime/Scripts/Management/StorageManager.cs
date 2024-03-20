using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class StorageManager : Singleton<StorageManager>
    {
        public Storage[] storages { get { return _storages.ToArray(); } }
        private List<Storage> _storages;

        public void Setup(Action<bool> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.user.token }
            };

            Request.Post(ApplicationManager.instance.backend.storageGet, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    StorageResponse storageResponse = JsonUtility.FromJson<StorageResponse>(response.data);

                    _storages = new List<Storage>();

                    foreach(StorageResponse.Data storage in storageResponse.datas)
                    {
                        _storages.Add(new Storage(storage.name, storage.type));
                    }

                    callback?.Invoke(true);
                }
                else
                {
                    callback?.Invoke(false);
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
                    { "token", UserManager.instance.user.token },
                    { "name", name },
                    { "type", typeof(T).Name }
                };

                Request.Post(ApplicationManager.instance.backend.storageDelete, formFields, (Request.Response response) =>
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
                { "token", UserManager.instance.user.token },
                { "name", name },
                { "type", typeof(T).Name }
            };

            Request.Post(ApplicationManager.instance.backend.storageLoad, formFields, (Request.Response response) =>
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
                { "type", typeof(T).Name },
                { "data", JsonUtility.ToJson(data) }
            };

            Request.Post(ApplicationManager.instance.backend.storageSave, formFields, (Request.Response response) =>
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

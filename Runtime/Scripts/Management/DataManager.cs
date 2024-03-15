using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class DataManager : Singleton<DataManager>
    {
        public Data[] datas { get { return _datas.ToArray(); } }
        private List<Data> _datas;

        public void Setup(Action<bool> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.user.token }
            };

            Request.Post(ApplicationManager.instance.backend.dataGet, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    DataResponse dataResponse = JsonUtility.FromJson<DataResponse>(response.data);

                    _datas = new List<Data>();

                    foreach(DataResponse.Data data in dataResponse.datas)
                    {
                        _datas.Add(new Data(data.name, data.type));
                    }

                    callback?.Invoke(true);
                }
                else
                {
                    callback?.Invoke(false);
                }
            });
        }

        public Data Get<T>(string name)
        {
            foreach(Data data in _datas)
            {
                if(data.name == name && data.type == typeof(T).Name)
                {
                    return data;
                }
            }

            return null;
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

                Request.Post(ApplicationManager.instance.backend.dataDelete, formFields, (Request.Response response) =>
                {
                    if(response.success)
                    {
                        _datas.Remove(Get<T>(name));

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

            Request.Post(ApplicationManager.instance.backend.dataLoad, formFields, (Request.Response response) =>
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

            Request.Post(ApplicationManager.instance.backend.dataSave, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    if (_datas.Find((x)=>x.name == name && x.type == typeof(T).Name) == null)
                    {
                        _datas.Add(new Data(name, typeof(T).Name));
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

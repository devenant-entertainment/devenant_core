using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class DataManager : Singleton<DataManager>
    {
        public Data[] datas { get { return _datas; } private set { _datas = value; } }
        private Data[] _datas;

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
                    DataResponse data = JsonUtility.FromJson<DataResponse>(response.data);

                    datas = new Data[data.datas.Length];

                    for(int i = 0; i < datas.Length; i++)
                    {
                        datas[i] = new Data(data.datas[i]);
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

            Request.Post(ApplicationManager.instance.backend.dataDelete, formFields, (Request.Response response) =>
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
                { "data", JsonUtility.ToJson(data) }
            };

            Request.Post(ApplicationManager.instance.backend.dataSave, formFields, (Request.Response response) =>
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

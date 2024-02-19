using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class ConfigurationManager : Singleton<ConfigurationManager>
    {
        [System.Serializable]
        public class Data
        {
            public readonly string version;
            public readonly string status;
        }

        public Data data { get { return _data; } private set { _data = value; } }
        private Data _data;

        public void Setup(Action<bool> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "game", UnityEngine.Application.productName }
            };

            Request.Post(Application.config.gameApiUrl + "config", formFields, ((Request.Response response) =>
            {
                if(response.success)
                {
                    data = JsonUtility.FromJson<Data>(response.data);

                    if(data.status == "active")
                    {
                        if(new Version(UnityEngine.Application.version).Compare(new Version(data.version)) != Version.Comparison.Greater)
                        {
                            callback?.Invoke(true);
                        }
                        else
                        {
                            MessageMenu.instance.Open("version_error", (() =>
                            {
                                callback?.Invoke(false);

                                UnityEngine.Application.OpenURL(Application.config.storeUrl);
                            }));
                        }
                    }
                    else
                    {
                        MessageMenu.instance.Open("maintenance_error", () =>
                        {
                            callback?.Invoke(false);
                        });
                    }
                }
                else
                {
                    MessageMenu.instance.Open(response.message, () =>
                    {
                        callback?.Invoke(false);
                    });
                }
            }));
        }
    }
}

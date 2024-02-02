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

        [SerializeField] private string remoteCall;

        public void Initialize(Action<bool> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "game", Application.productName }
            };

            Request.Post(remoteCall + "config/get.php", formFields, ((Request.Response response) =>
            {
                if(response.success)
                {
                    data = JsonUtility.FromJson<Data>(response.data);

                    if(data.status == "active")
                    {
                        if(new Version(Application.version).Compare(new Version(data.version)) != Version.Comparison.Greater)
                        {
                            callback?.Invoke(true);
                        }
                        else
                        {
                            MessageMenu.instance.Open("version_error", (() =>
                            {
                                callback?.Invoke(false);

                                Application.OpenURL(ApplicationManager.instance.applicationData.GetStoreUrl());
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

using UnityEngine;

namespace Devenant
{
    public class ConfigurationManager : Singleton<ConfigurationManager>
    {
        public Configuration configuration { get { return _configuration; } private set { _configuration = value; } }
        private Configuration _configuration;

        public void Setup(Action<bool> callback)
        {
            Request.Get(ApplicationManager.instance.config.endpoints.config, ((Request.Response response) =>
            {
                if(response.success)
                {
                    configuration = new Configuration(JsonUtility.FromJson<ConfigurationResponse>(response.data));

                    switch(configuration.status)
                    {
                        case ConfigurationStatus.Active:

                            if(new Version(UnityEngine.Application.version).Compare(configuration.version) != Version.Comparison.Greater)
                            {
                                callback?.Invoke(true);
                            }
                            else
                            {
                                MessageMenu.instance.Open("version_error", (() =>
                                {
                                    callback?.Invoke(false);

                                    UnityEngine.Application.OpenURL(ApplicationManager.instance.config.storeUrl);
                                }));
                            }

                            break;

                        case ConfigurationStatus.Inactive:

                            MessageMenu.instance.Open("maintenance_error", () =>
                            {
                                callback?.Invoke(false);
                            });

                            break;
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

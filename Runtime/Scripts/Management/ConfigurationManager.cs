using UnityEngine;

namespace Devenant
{
    public class ConfigurationManager : Singleton<ConfigurationManager>
    {
        public Configuration configuration { get { return _configuration; } private set { _configuration = value; } }
        private Configuration _configuration;

        public void Setup(Action callback)
        {
            Request.Get(ApplicationManager.instance.backend.configuration, ((Request.Response response) =>
            {
                if(response.success)
                {
                    configuration = new Configuration(JsonUtility.FromJson<ConfigurationResponse>(response.data));

                    switch(configuration.status)
                    {
                        case ConfigurationStatus.Active:

                            if(new Version(UnityEngine.Application.version).Compare(configuration.version) != Version.Comparison.Greater)
                            {
                                callback?.Invoke();
                            }
                            else
                            {
                                MessageMenu.instance.Open("dialogue_version", ((bool success) =>
                                {
                                    if(success)
                                    {
                                        UnityEngine.Application.OpenURL(ApplicationManager.instance.application.storeUrl);
                                    }
                                    else
                                    {
                                        ApplicationManager.instance.Exit();
                                    }
                                }));
                            }

                            break;

                        case ConfigurationStatus.Inactive:

                            MessageMenu.instance.Open("error_maintenance", () =>
                            {
                                ApplicationManager.instance.Exit();
                            });

                            break;
                    }
                }
                else
                {
                    MessageMenu.instance.Open(response.message, () =>
                    {
                        ApplicationManager.instance.Exit();
                    });
                }
            }));
        }
    }
}

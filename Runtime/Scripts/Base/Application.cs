using Unity.Services.Core;
using Unity.Services.Core.Environments;

namespace Devenant
{
    public static class Application
    {
        public class Config
        {
            public enum Environment
            {
                Production,
                Development
            }

            public readonly Environment environment;

            public readonly string apiUrl;
            public readonly string gameUrl;
            public readonly string legalUrl;
            public readonly string storeUrl;

            public Config (Environment environment, string apiUrl, string gameUrl, string legalUrl, string storeUrl)
            {
                this.environment = environment;
                this.apiUrl = apiUrl;
                this.gameUrl = gameUrl;
                this.legalUrl = legalUrl;
                this.storeUrl = storeUrl;
            }
        }

        public static Config config { get { return _config; } }
        private static Config _config;

        public static void Initialize(Config config, Action callback = null)
        {
            _config = config;

            Setup(callback);

            async void Setup(Action callback)
            {
                InitializationOptions options = new InitializationOptions();
                options.SetEnvironmentName(config.environment.ToString());

                await UnityServices.InitializeAsync(options);

                LoadingMenu.instance.Open(() =>
                {
                    LocalizationManager.instance.Setup(() =>
                    {
                        SettingsManager.instance.Load();

                        ConfigurationManager.instance.Setup((bool success) =>
                        {
                            if(success)
                            {
                                PurchaseManager.instance.Setup((bool success) =>
                                {
                                    if(success)
                                    {
                                        UserManager.instance.AutoLogin((bool success) =>
                                        {
                                            LoadingMenu.instance.Close(() =>
                                            {
                                                if(success)
                                                {
                                                    if(UserManager.instance.data.validated)
                                                    {
                                                        callback?.Invoke();
                                                    }
                                                    else
                                                    {
                                                        UserValidationMenu.instance.Open(() =>
                                                        {
                                                            callback?.Invoke();
                                                        });
                                                    }
                                                }
                                                else
                                                {
                                                    UserLoginMenu.instance.Open(() =>
                                                    {
                                                        if(UserManager.instance.data.validated)
                                                        {
                                                            callback?.Invoke();
                                                        }
                                                        else
                                                        {
                                                            UserValidationMenu.instance.Open(() =>
                                                            {
                                                                callback?.Invoke();
                                                            });
                                                        }
                                                    });
                                                }
                                            });
                                        });
                                    }
                                    else
                                    {
                                        MessageMenu.instance.Open("error", () =>
                                        {
                                            Exit();
                                        });
                                    }
                                });
                            }
                            else
                            {
                                LoadingMenu.instance.Close();

                                MessageMenu.instance.Open("error", () =>
                                {
                                    Exit();
                                });
                            }
                        });
                    });
                });
            }
        }

        public static void Exit()
        {
            UnityEngine.Application.Quit();
        }
    }
}

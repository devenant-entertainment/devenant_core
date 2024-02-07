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
#if(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
                    SteamManager.instance.Setup((bool success) =>
                    {
                        if(success)
                        {
#endif
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
                                                AchievementManager.instance.Setup((bool success) =>
                                                {
                                                    if(success)
                                                    {
                                                        UserManager.instance.AutoLogin((bool success) =>
                                                        {
                                                            LoadingMenu.instance.Close(() =>
                                                            {
                                                                if(success)
                                                                {
                                                                    OnLogin(callback);
                                                                }
                                                                else
                                                                {
                                                                    UserLoginMenu.instance.Open(() =>
                                                                    {
                                                                        OnLogin(callback);
                                                                    });
                                                                }
                                                            });
                                                        });
                                                    }
                                                    else
                                                    {
                                                        Exit();
                                                    }
                                                });
                                            }
                                            else
                                            {
                                                Exit();
                                            }
                                        });
                                    }
                                    else
                                    {
                                        Exit();
                                    }
                                });
                            });
#if(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
                        }
                        else
                        {
                            Exit();
                        }
                    });
#endif
                });
            }

            void OnLogin(Action callback)
            {
                switch(UserManager.instance.data.status)
                {
                    case "active":

                        callback?.Invoke();

                        break;

                    case "unvalidated":

                        UserValidationMenu.instance.Open(() =>
                        {
                            callback?.Invoke();
                        });

                        break;

                    case "banned":

                        MessageMenu.instance.Open("user_banned", () =>
                        {
                            Exit();
                        });

                        break;
                }
            }

            void Exit()
            {
                LoadingMenu.instance.Close(() =>
                {
                    MessageMenu.instance.Open("error", () =>
                    {
                        Exit();
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

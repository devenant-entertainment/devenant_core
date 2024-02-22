using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

namespace Devenant
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        public Application config { get { return _config; } }
        private Application _config;

        public void Initialize(Application config, Purchase.Info[] purchases, Achievement.Info[] achievements, Action callback = null)
        {
            _config = config;

            Setup(callback);

            async void Setup(Action callback)
            {
                InitializationOptions options = new InitializationOptions();
                options.SetEnvironmentName(config.environment.ToString().ToLower());

                await UnityServices.InitializeAsync(options);

                await AuthenticationService.Instance.SignInAnonymouslyAsync();

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
                                        PurchaseManager.instance.Setup(purchases, (bool success) =>
                                        {
                                            if(success)
                                            {
                                                AchievementManager.instance.Setup(achievements, (bool success) =>
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
                switch(UserManager.instance.user.status)
                {
                    case UserStatus.Active:

                        callback?.Invoke();

                        break;

                    case UserStatus.Unvalidated:

                        UserValidationMenu.instance.Open(() =>
                        {
                            callback?.Invoke();
                        });

                        break;

                    case UserStatus.Banned:

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

        public void Exit()
        {
            UnityEngine.Application.Quit();
        }
    }
}

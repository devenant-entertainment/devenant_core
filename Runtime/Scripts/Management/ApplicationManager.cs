using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

namespace Devenant
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        public Application application { get { return _application; } private set { _application = value; } }
        private Application _application;

        public Backend backend { get { return _backend; } private set { _backend = value; } }
        private Backend _backend;

        public void Initialize(ApplicationData application, BackendData backend, PurchaseData[] purchases, AchievementData[] achievements, AvatarData[] avatars, Action callback = null)
        {
            this.application = new Application(application);

            this.backend = new Backend(backend);

            Setup(callback);

            async void Setup(Action callback)
            {
                InitializationOptions options = new InitializationOptions();
                options.SetEnvironmentName(this.application.environment.ToString().ToLower());

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
                                                        AvatarManager.instance.Setup(avatars);

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
                                                                    UserLoginMenu.instance.Open((bool success) =>
                                                                    {
                                                                        if(success)
                                                                        {
                                                                            OnLogin(callback);
                                                                        }
                                                                        else
                                                                        {
                                                                            Exit();
                                                                        }
                                                                    });
                                                                }
                                                            });
                                                        });
                                                    }
                                                    else
                                                    {
                                                        ShowError();
                                                    }
                                                });
                                            }
                                            else
                                            {
                                                ShowError();
                                            }
                                        });
                                    }
                                    else
                                    {
                                        ShowError();
                                    }
                                });
                            });
#if(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
                        }
                        else
                        {
                            ShowError();
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

                        MessageMenu.instance.Open("user_unvalidated", (bool success) =>
                        {
                            if(success)
                            {
                                UserSendCodeMenu.instance.Open((bool success) =>
                                {
                                    if(success)
                                    {
                                        UserValidateMenu.instance.Open((bool success) =>
                                        {
                                            if(success)
                                            {
                                                callback?.Invoke();
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

                        break;

                    case UserStatus.Banned:

                        MessageMenu.instance.Open("user_banned", () =>
                        {
                            Exit();
                        });

                        break;

                    case UserStatus.Deleted:

                        MessageMenu.instance.Open("user_deleted", (bool success) =>
                        {
                            if(success)
                            {
                                UserSendCodeMenu.instance.Open((bool success) =>
                                {
                                    if(success)
                                    {
                                        UserRestoreMenu.instance.Open((bool success) =>
                                        {
                                            if(success)
                                            {
                                                callback?.Invoke();
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

                        break;
                }
            }

            void ShowError()
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

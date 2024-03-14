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

        public void Initialize(Action callback = null)
        {
            AssetManager.instance.Get((SOApplication application) =>
            {
                this.application = new Application(application);

                AssetManager.instance.Get((SOBackend backend) =>
                {
                    this.backend = new Backend(backend);

                    Setup(callback);
                });
            });

            async void Setup(Action callback)
            {
                InitializationOptions options = new InitializationOptions();
                options.SetEnvironmentName(this.application.environment.ToString().ToLower());

                await UnityServices.InitializeAsync(options);

                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                LoadingMenu.instance.Open(() =>
                {
                    AudioManager.instance.Setup();

#if(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
                    SteamManager.instance.Setup((bool success) =>
                    {
                        if(success)
                        {
#endif
                            LocalizationManager.instance.Setup(() =>
                            {
                                SettingsManager.instance.Setup();

                                ConfigurationManager.instance.Setup(() =>
                                {
                                    UserManager.instance.AutoLogin((bool success) =>
                                    {
                                        if(success)
                                        {
                                            OnLogin(callback);
                                        }
                                        else
                                        {
                                            LoadingMenu.instance.Close(() =>
                                            {
                                                UserLoginMenu.instance.Open(() =>
                                                {
                                                    LoadingMenu.instance.Open(() =>
                                                    {
                                                        OnLogin(callback);
                                                    });
                                                });
                                            });
                                        }
                                    });
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
                PurchaseManager.instance.Setup((bool success) =>
                {
                    if(success)
                    {
                        AchievementManager.instance.Setup((bool success) =>
                        {
                            if(success)
                            {
                                AvatarManager.instance.Setup(() =>
                                {
                                    DataManager.instance.Setup((bool success) =>
                                    {
                                        if(success)
                                        {
                                            LoadingMenu.instance.Close(() =>
                                            {
                                                switch(UserManager.instance.user.status)
                                                {
                                                    case UserStatus.Active:

                                                        callback?.Invoke();

                                                        break;

                                                    case UserStatus.Unvalidated:

                                                        MessageMenu.instance.Open("dialogue_user_unvalidated", (bool success) =>
                                                        {
                                                            if(success)
                                                            {
                                                                UserSendCodeMenu.instance.Open((bool success) =>
                                                                {
                                                                    if(success)
                                                                    {
                                                                        UserActivateMenu.instance.Open((bool success) =>
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

                                                        MessageMenu.instance.Open("dialogue_user_banned", (bool success) =>
                                                        {
                                                            if(success)
                                                            {
                                                                UnityEngine.Application.OpenURL(application.supportUrl);
                                                            }
                                                            else
                                                            {
                                                                Exit();
                                                            }
                                                        });

                                                        break;

                                                    case UserStatus.Deleted:

                                                        MessageMenu.instance.Open("dialogue_user_deleted", (bool success) =>
                                                        {
                                                            if(success)
                                                            {
                                                                UserSendCodeMenu.instance.Open((bool success) =>
                                                                {
                                                                    if(success)
                                                                    {
                                                                        UserActivateMenu.instance.Open((bool success) =>
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
                                                                        ShowError();
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
                                            });
                                        }
                                        else
                                        {
                                            ShowError();
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

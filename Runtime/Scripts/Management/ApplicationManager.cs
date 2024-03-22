using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace Devenant
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        public Application application { get { return _application; } private set { _application = value; } }
        private Application _application;

        public Backend backend { get { return _backend; } private set { _backend = value; } }
        private Backend _backend;

        public Status status { get { return _status; } private set { _status = value; } }
        private Status _status;

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

                options.SetEnvironmentName(application.environment.ToString().ToLower());

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

                                SetupStatus(() =>
                                {
                                    Login(() =>
                                    {
                                        SetupData(() =>
                                        {
                                            SetupUser(callback);
                                        });
                                    });
                                });
                            });
#if(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
                        }
                        else
                        {
                            LoadingMenu.instance.Close(() =>
                            {
                                MessageMenu.instance.Open("error", () =>
                                {
                                    Exit();
                                });
                            });
                        }
                    });
#endif
                });
            }
        }

        private void SetupStatus(Action callback)
        {
            Request.Get(backend.status, ((Request.Response response) =>
            {
                if(response.success)
                {
                    status = new Status(JsonUtility.FromJson<StatusResponse>(response.data));

                    switch(status.status)
                    {
                        case ApplicationStatus.Active:

                            if(new Version(UnityEngine.Application.version).Compare(status.version) != Version.Comparison.Greater)
                            {
                                callback?.Invoke();
                            }
                            else
                            {
                                MessageMenu.instance.Open("dialogue_version", ((bool success) =>
                                {
                                    if(success)
                                    {
                                        UnityEngine.Application.OpenURL(application.storeUrl);
                                    }
                                    else
                                    {
                                        Exit();
                                    }
                                }));
                            }

                            break;

                        case ApplicationStatus.Inactive:

                            MessageMenu.instance.Open("error_maintenance", () =>
                            {
                                Exit();
                            });

                            break;
                    }
                }
                else
                {
                    MessageMenu.instance.Open(response.message, () =>
                    {
                        Exit();
                    });
                }
            }));
        }

        private void Login(Action callback)
        {
            UserManager.instance.AutoLogin((bool success) =>
            {
                if(success)
                {
                    callback?.Invoke();
                }
                else
                {
                    LoadingMenu.instance.Close(() =>
                    {
                        UserLoginMenu.instance.Open(() =>
                        {
                            LoadingMenu.instance.Open(() =>
                            {
                                callback?.Invoke();
                            });
                        });
                    });
                }
            });
        }

        private void SetupData(Action callback)
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
                                StorageManager.instance.Setup((bool success) =>
                                {
                                    if(success)
                                    {
                                        LoadingMenu.instance.Close(() =>
                                        {
                                            callback?.Invoke();
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
                    MessageMenu.instance.Open("error", () =>
                    {
                        Exit();
                    });
                }
            });
        }

        private void SetupUser(Action callback)
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

        public void Exit()
        {
            UnityEngine.Application.Quit();
        }
    }
}

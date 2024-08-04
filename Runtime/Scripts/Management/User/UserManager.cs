using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Devenant
{
    [RequireComponent(typeof(InitializableObject))]
    public class UserManager : Singleton<UserManager>, IInitializable
    {
        public static Action<UserData> onUserUpdated;

        private const string emailKey = "UserManager.Email";
        private const string passwordKey = "UserManager.Password";
        private const string identifierKey = "UserManager.Identifier";

        public UserData data { get { return _data; } private set { _data = value; } }
        private UserData _data;

        public void Initialize(Action<InitializationResponse> callback)
        {
            TryAutoLogin((bool success) =>
            {
                if (success)
                {
                    ValidateUser((bool success) =>
                    {
                        callback?.Invoke(new InitializationResponse(success));
                    });
                }
                else
                {
                    UserLoginMenu.instance.Open((bool success) =>
                    {
                        if (success)
                        {
                            ValidateUser((bool success) =>
                            {
                                callback?.Invoke(new InitializationResponse(success));
                            });
                        }
                        else
                        {
                            callback?.Invoke(new InitializationResponse(success));
                        }
                    });
                }
            });
            
            void TryAutoLogin(Action<bool> callback)
            {
                if (PlayerPrefs.HasKey(emailKey) && PlayerPrefs.HasKey(passwordKey))
                {
                    Login(PlayerPrefs.GetString(emailKey), PlayerPrefs.GetString(passwordKey), false, (Request.Response response) =>
                    {
                        callback?.Invoke(response.success);
                    });
                }
                else if (PlayerPrefs.HasKey(identifierKey))
                {
                    LoginGuest((Request.Response response) =>
                    {
                        callback?.Invoke(response.success);
                    });
                }
                else
                {
                    callback?.Invoke(false);
                }
            }

            void ValidateUser(Action<bool> callback)
            {
                switch (data.status)
                {
                    case UserStatus.Active:

                        callback?.Invoke(true);

                        break;

                    case UserStatus.Unvalidated:

                        MessageMenu.instance.Open("dialogue_user_unvalidated", (bool success) =>
                        {
                            if (success)
                            {
                                UserSendCodeMenu.instance.Open((bool success) =>
                                {
                                    if (success)
                                    {
                                        UserActivateMenu.instance.Open((bool success) =>
                                        {
                                            callback?.Invoke(success);
                                        });
                                    }
                                    else
                                    {
                                        callback?.Invoke(false);
                                    }
                                });
                            }
                            else
                            {
                                callback?.Invoke(false);
                            }
                        });

                        break;

                    case UserStatus.Banned:

                        MessageMenu.instance.Open("dialogue_user_banned", (bool success) =>
                        {
                            if (success)
                            {
                                Application.OpenURL(ApplicationManager.instance.data.supportUrl);
                            }

                            callback?.Invoke(false);
                        });

                        break;

                    case UserStatus.Deleted:

                        MessageMenu.instance.Open("dialogue_user_deleted", (bool success) =>
                        {
                            if (success)
                            {
                                UserSendCodeMenu.instance.Open((bool success) =>
                                {
                                    if (success)
                                    {
                                        UserActivateMenu.instance.Open((bool success) =>
                                        {
                                            callback?.Invoke(success);
                                        });
                                    }
                                    else
                                    {
                                        callback?.Invoke(false);
                                    }
                                });
                            }
                            else
                            {
                                callback?.Invoke(false);
                            }
                        });

                        break;
                }
            }
        }

        public void Activate(string code, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", data.token },
                { "code", code }
            };

            Request.Post(BackendManager.instance.data.userActivate, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    data.status = UserStatus.Active;

                    onUserUpdated?.Invoke(data);
                }

                callback?.Invoke(response);
            });
        }

        public void Delete(string code, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", data.token },
                { "code", code }
            };

            Request.Post(BackendManager.instance.data.userDelete, formFields, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void Login(string email, string password, bool rememberData, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "email", email },
                { "password", password }
            };

            Request.Post(BackendManager.instance.data.userLogin, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    data = new UserData(JsonUtility.FromJson<UserDataResponse>(response.data));

                    onUserUpdated?.Invoke(data);

                    if(rememberData)
                    {
                        PlayerPrefs.SetString(emailKey, email);
                        PlayerPrefs.SetString(passwordKey, password);
                    }

                    PlayerPrefs.DeleteKey(identifierKey);
                }

                callback?.Invoke(response);
            });
        }

        public void LoginGuest(Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "identifier", SystemInfo.deviceUniqueIdentifier }
            };

            Request.Post(BackendManager.instance.data.userLoginGuest, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    data = new UserData(JsonUtility.FromJson<UserDataResponse>(response.data));

                    onUserUpdated?.Invoke(data);

                    PlayerPrefs.DeleteKey(emailKey);
                    PlayerPrefs.DeleteKey(passwordKey);

                    PlayerPrefs.SetString(identifierKey, SystemInfo.deviceUniqueIdentifier);
                }

                callback?.Invoke(response);
            });
        }

        public void Register(string nickname, string email, string password, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "nickname", nickname },
                { "email", email },
                { "password", password }
            };

            Request.Post(BackendManager.instance.data.userRegister, formFields, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void SendCode(string email, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "email", email }
            };

            Request.Post(BackendManager.instance.data.userSendCode, formFields, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void UpdateAvatar(string avatar, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", data.token },
                { "avatar", avatar }
            };

            Request.Post(BackendManager.instance.data.userUpdateAvatar, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    data.avatar = avatar;

                    onUserUpdated?.Invoke(data);
                }

                callback?.Invoke(response);
            });
        }

        public void UpdateEmail(string code, string email, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", data.token },
                { "code", code },
                { "email", email }
            };

            Request.Post(BackendManager.instance.data.userUpdateEmail, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    data.email = email;

                    PlayerPrefs.SetString(emailKey, email);

                    onUserUpdated?.Invoke(data);
                }

                callback?.Invoke(response);
            });
        }

        public void UpdateGuest(string email, string password, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", data.token },
                { "email", email },
                { "password", password }
            };

            Request.Post(BackendManager.instance.data.userUpdateGuest, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    data.email = email;

                    PlayerPrefs.SetString(emailKey, email);
                    PlayerPrefs.SetString(passwordKey, password);

                    PlayerPrefs.DeleteKey(identifierKey);
                }

                callback?.Invoke(response);
            });
        }

        public void UpdatePassword(string code, string password, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "code", code },
                { "password", password }
            };

            Request.Post(BackendManager.instance.data.userUpdatePassword, formFields, (Request.Response response) =>
            {
                PlayerPrefs.SetString(passwordKey, password);

                callback?.Invoke(response);
            });
        }

        public void UpdateNickname(string code, string nickname, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", data.token },
                { "code", code },
                { "nickname", nickname }
            };

            Request.Post(BackendManager.instance.data.userUpdateNickname, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    data.nickname = nickname;

                    onUserUpdated?.Invoke(data);
                }

                callback?.Invoke(response);
            });
        }

        public void Logout()
        {
            data = null;

            PlayerPrefs.DeleteKey(emailKey);
            PlayerPrefs.DeleteKey(passwordKey);
            PlayerPrefs.DeleteKey(identifierKey);
        }

        public bool ValidateNickname(string nickname)
        {
            return Regex.IsMatch(nickname, "^[A-Za-z0-9._-]{2,}$");
        }

        public bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, "^\\S+@\\S+\\.\\S+$");
        }

        public bool ValidatePassword(string password)
        {
            return Regex.IsMatch(password, "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$");
        }
    }
}

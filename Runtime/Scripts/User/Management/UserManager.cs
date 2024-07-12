using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Devenant
{
    public class UserManager : Singleton<UserManager>
    {
        public static Action<User> onUserUpdated;

        private const string emailKey = "UserManager.Email";
        private const string passwordKey = "UserManager.Password";

        private const string identifierKey = "UserManager.Identifier";

        public User user { get { return _user; } private set { _user = value; } }
        private User _user;

        public void AutoLogin(Action<bool> callback)
        {
            if(PlayerPrefs.HasKey(emailKey) && PlayerPrefs.HasKey(passwordKey))
            {
                Login(PlayerPrefs.GetString(emailKey), PlayerPrefs.GetString(passwordKey), false, (Request.Response response) =>
                {
                    callback?.Invoke(response.success);
                });
            }
            else if(PlayerPrefs.HasKey(identifierKey))
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

        public void Activate(string code, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", user.token },
                { "code", code }
            };

            Request.Post(ApplicationManager.instance.backend.userActivate, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    user.status = UserStatus.Active;

                    onUserUpdated?.Invoke(user);
                }

                callback?.Invoke(response);
            });
        }

        public void Delete(string code, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", user.token },
                { "code", code }
            };

            Request.Post(ApplicationManager.instance.backend.userDelete, formFields, (Request.Response response) =>
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

            Request.Post(ApplicationManager.instance.backend.userLogin, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    user = new User(JsonUtility.FromJson<UserResponse>(response.data));

                    onUserUpdated?.Invoke(user);

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

            Request.Post(ApplicationManager.instance.backend.userLoginGuest, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    user = new User(JsonUtility.FromJson<UserResponse>(response.data));

                    onUserUpdated?.Invoke(user);

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

            Request.Post(ApplicationManager.instance.backend.userRegister, formFields, (Request.Response response) =>
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

            Request.Post(ApplicationManager.instance.backend.userSendCode, formFields, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void UpdateAvatar(string avatar, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", user.token },
                { "avatar", avatar }
            };

            Request.Post(ApplicationManager.instance.backend.userUpdateAvatar, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    user.avatar = avatar;

                    onUserUpdated?.Invoke(user);
                }

                callback?.Invoke(response);
            });
        }

        public void UpdateEmail(string code, string email, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", user.token },
                { "code", code },
                { "email", email }
            };

            Request.Post(ApplicationManager.instance.backend.userUpdateEmail, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    user.email = email;

                    PlayerPrefs.SetString(emailKey, email);

                    onUserUpdated?.Invoke(user);
                }

                callback?.Invoke(response);
            });
        }

        public void UpdateGuest(string email, string password, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", user.token },
                { "email", email },
                { "password", password }
            };

            Request.Post(ApplicationManager.instance.backend.userUpdateGuest, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    user.email = email;

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

            Request.Post(ApplicationManager.instance.backend.userUpdatePassword, formFields, (Request.Response response) =>
            {
                PlayerPrefs.SetString(passwordKey, password);

                callback?.Invoke(response);
            });
        }

        public void UpdateNickname(string code, string nickname, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", user.token },
                { "code", code },
                { "nickname", nickname }
            };

            Request.Post(ApplicationManager.instance.backend.userUpdateNickname, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    user.nickname = nickname;

                    onUserUpdated?.Invoke(user);
                }

                callback?.Invoke(response);
            });
        }

        public void Logout()
        {
            user = null;

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

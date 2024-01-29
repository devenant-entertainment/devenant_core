using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class UserManager : Singleton<UserManager>
    {
        public static Action<Data> onUserUpdated;

        private const string emailKey = "UserManager.Email";
        private const string passwordKey = "UserManager.Password";

        [SerializeField] private string remoteCall;

        [System.Serializable]
        public class Data
        {
            public string token;
            public string nickname;
            public string avatar;
            public string email;
            public string type;
            public string status;
        }

        public Data data { get { return _data; } private set { _data = value; } }
        private Data _data;

        public void AutoLogin(Action<bool> callback)
        {
            if(PlayerPrefs.HasKey(emailKey) && PlayerPrefs.HasKey(passwordKey))
            {
                Login(PlayerPrefs.GetString(emailKey), PlayerPrefs.GetString(passwordKey), false, (Request.Response response) => 
                {
                    callback?.Invoke(response.success);
                });
            }
            else
            {
                callback?.Invoke(false);
            }
        }

        public void Login(string email, string password, bool rememberData, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "email", email },
                { "password", password }
            };

            Request.Post(remoteCall + "login.php", formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    data = JsonUtility.FromJson<Data>(response.data);

                    if(rememberData)
                    {
                        PlayerPrefs.SetString(emailKey, email);
                        PlayerPrefs.SetString(passwordKey, password);
                    }
                }

                callback?.Invoke(response);
            });
        }

        public void Register(string nickname, string avatar, string email, string password, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "nickname", nickname },
                { "avatar", avatar },
                { "email", email },
                { "password", password }
            };

            Request.Post(remoteCall + "register.php", formFields, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void ActivationGet(Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", data.token },
            };

            Request.Post(remoteCall + "activation_get.php", formFields, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void ActivationUpdate(string code, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", data.token },
                { "code", code }
            };

            Request.Post(remoteCall + "activation_update.php", formFields, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void UpdateNickname(string nickname, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", data.token },
                { "nickname", nickname }
            };

            Request.Post(remoteCall + "update_nickname.php", formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    data.nickname = nickname;
                    onUserUpdated?.Invoke(data);
                }

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

            Request.Post(remoteCall + "update_avatar.php", formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    data.avatar = avatar;
                    onUserUpdated?.Invoke(data);
                }

                callback?.Invoke(response);
            });
        }

        public void UpdateEmailGet(string email, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", data.token },
                { "email", email }
            };

            Request.Post(remoteCall + "update_email_get.php", formFields, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void UpdateEmailUpdate(string code, string email, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", data.token },
                { "code", code },
                { "email", email }
            };

            Request.Post(remoteCall + "update_email_update.php", formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    data.email = email;

                    onUserUpdated?.Invoke(data);
                }

                callback?.Invoke(response);
            });
        }

        public void UpdatePasswordGet(string email, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "email", email }
            };

            Request.Post(remoteCall + "update_password_get.php", formFields, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void UpdatePasswordUpdate(string code, string email, string password, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "code", code },
                { "email", email },
                { "password", password }
            };

            Request.Post(remoteCall + "update_password_update.php", formFields, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void DeleteGet(string email, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", data.token },
                { "email", email }
            };

            Request.Post(remoteCall + "delete_get.php", formFields, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void DeleteUpdate(string code, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", data.token },
                { "code", code }
            };

            Request.Post(remoteCall + "delete_update.php", formFields, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void Logout()
        {
            PlayerPrefs.DeleteKey(emailKey);
            PlayerPrefs.DeleteKey(passwordKey);
        }
    }
}

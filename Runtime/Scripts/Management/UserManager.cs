using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Devenant
{
    public class UserManager : Singleton<UserManager>
    {
        public static Action<Data> onUserUpdated;

        private const string emailKey = "UserManager.Email";
        private const string passwordKey = "UserManager.Password";

        [System.Serializable]
        public class Data
        {
            public string token;
            public string nickname;
            public string avatar;
            public string email;
            public string type;
            public bool validated;
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

        public void Code(Action<Request.Response> callback)
        {
            Request.Get(Application.config.apiUrl + "user/code", data.token, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void Delete(string code, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "code", code }
            };

            Request.Post(Application.config.apiUrl + "user/delete", formFields, data.token, (Request.Response response) =>
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

            Request.Post(Application.config.apiUrl + "user/login", formFields, (Request.Response response) =>
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

        public void Register(string nickname, string email, string password, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "nickname", nickname },
                { "email", email },
                { "password", password }
            };

            Request.Post(Application.config.apiUrl + "user/register", formFields, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void UpdateAvatar(string avatar, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "avatar", avatar }
            };

            Request.Post(Application.config.apiUrl + "user/update_avatar", formFields, data.token, (Request.Response response) =>
            {
                if(response.success)
                {
                    data.avatar = avatar;

                    onUserUpdated?.Invoke(data);
                }

                callback?.Invoke(response);
            });
        }

        public void UpdateEmail(string email, string code, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "email", email },
                { "code", code }
            };

            Request.Post(Application.config.apiUrl + "user/update_email", formFields, data.token, (Request.Response response) =>
            {
                if(response.success)
                {
                    data.email = email;

                    onUserUpdated?.Invoke(data);
                }

                callback?.Invoke(response);
            });
        }

        public void UpdatePassword(string password, string code, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "password", password },
                { "code", code }
            };

            Request.Post(Application.config.apiUrl + "user/update_password", formFields, data.token, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void UpdateNickname(string nickname, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "nickname", nickname }
            };

            Request.Post(Application.config.apiUrl + "user/update_nickname", formFields, data.token, (Request.Response response) =>
            {
                if(response.success)
                {
                    data.nickname = nickname;
                    onUserUpdated?.Invoke(data);
                }

                callback?.Invoke(response);
            });
        }

        public void Validate(string code, Action<Request.Response> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "code", code }
            };

            Request.Post(Application.config.apiUrl + "user/validate", formFields, data.token, (Request.Response response) =>
            {
                callback?.Invoke(response);
            });
        }

        public void Logout()
        {
            PlayerPrefs.DeleteKey(emailKey);
            PlayerPrefs.DeleteKey(passwordKey);
        }

        public bool ValidateNickname(string nickname)
        {
            return Regex.IsMatch(nickname, "^(?=.*?[A-Z])(?=.*?[a-z]).{2,}$");
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

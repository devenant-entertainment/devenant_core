using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserLoginMenu : Menu<UserLoginMenu>
    {
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private Toggle rememberToggle;
        [Space]
        [SerializeField] private Button loginButton;
        [SerializeField] private Button updatePasswordButton;
        [SerializeField] private Button registerButton;
        [Space]
        [SerializeField] private Button closeButton;

        public void Open(Action<bool> callback = null)
        {
            emailInputField.text = string.Empty;
            emailInputField.contentType = TMP_InputField.ContentType.EmailAddress;
            emailInputField.characterLimit = 256;

            passwordInputField.text = string.Empty;
            passwordInputField.contentType = TMP_InputField.ContentType.Password;

            rememberToggle.isOn = true;

            loginButton.onClick.RemoveAllListeners();
            loginButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(emailInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("user_empty_fields"));

                    return;
                }

                if(string.IsNullOrEmpty(passwordInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("user_empty_fields"));

                    return;
                }

                if(!UserManager.instance.ValidatePassword(passwordInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("user_invalid_password"));

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.Login(emailInputField.text, passwordInputField.text, rememberToggle.isOn, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                Close(() => 
                                {
                                    callback?.Invoke(true);
                                });
                            }
                            else
                            {
                                NotificationMenu.instance.Open(new Notification(response.message));
                            }
                        });
                    });
                });
            });

            updatePasswordButton.onClick.RemoveAllListeners();
            updatePasswordButton.onClick.AddListener(() =>
            {
                UserSendCodeMenu.instance.Open((bool success) =>
                {
                    if(success)
                    {
                        UserUpdatePasswordMenu.instance.Open();
                    }
                });
            });

            registerButton.onClick.RemoveAllListeners();
            registerButton.onClick.AddListener(() =>
            {
                UserRegisterMenu.instance.Open();
            });

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                Close(() =>
                {
                    callback?.Invoke(false);
                });
            });

            base.Open();
        }
    }
}

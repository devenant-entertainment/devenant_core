using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserLoginMenu : Menu<UserLoginMenu>
    {
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private Button passwordShowButton;

        [SerializeField] private Toggle rememberToggle;
        
        [SerializeField] private Button loginButton;
        [SerializeField] private Button updatePasswordButton;
        [SerializeField] private Button registerButton;

        [SerializeField] private Button closeButton;

        public override void Open(Action callback = null)
        {
            emailInputField.text = string.Empty;
            emailInputField.contentType = TMP_InputField.ContentType.EmailAddress;
            emailInputField.characterLimit = 256;

            passwordInputField.text = string.Empty;
            passwordInputField.contentType = TMP_InputField.ContentType.Password;

            passwordShowButton.onClick.RemoveAllListeners();
            passwordShowButton.onClick.AddListener(() =>
            {
                passwordInputField.DeactivateInputField();
                passwordInputField.contentType = passwordInputField.contentType == TMP_InputField.ContentType.Password ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
                passwordInputField.ActivateInputField();
            });

            rememberToggle.isOn = true;

            loginButton.onClick.RemoveAllListeners();
            loginButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(emailInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("error_field_empty"));

                    return;
                }

                if(string.IsNullOrEmpty(passwordInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("error_field_empty"));

                    return;
                }

                if(!UserManager.instance.ValidatePassword(passwordInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("error_field_password"));

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
                                    callback?.Invoke();
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
                Close();
            });

            base.Open();
        }
    }
}

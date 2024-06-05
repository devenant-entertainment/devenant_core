using Devenant.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserRegisterMenu : Menu<UserRegisterMenu>
    {
        [SerializeField] private TMP_InputField nicknameInputField;
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        
        [SerializeField] private Toggle legalToggle;
        [SerializeField] private Button legalButton;
        
        [SerializeField] private Button registerButton;
        
        [SerializeField] private Button closeButton;

        public void Open(Action<bool> callback = null)
        {
            nicknameInputField.text = string.Empty;
            nicknameInputField.contentType = TMP_InputField.ContentType.Name;
            nicknameInputField.characterLimit = 32;

            emailInputField.text = string.Empty;
            emailInputField.contentType = TMP_InputField.ContentType.EmailAddress;
            emailInputField.characterLimit = 256;

            passwordInputField.text = string.Empty;
            passwordInputField.contentType = TMP_InputField.ContentType.Password;

            legalToggle.isOn = false;

            legalButton.onClick.RemoveAllListeners();
            legalButton.onClick.AddListener(() =>
            {
                UnityEngine.Application.OpenURL(ApplicationManager.instance.application.legalUrl);
            });

            registerButton.onClick.RemoveAllListeners();
            registerButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(nicknameInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("error_field_empty"));

                    return;
                }

                if(!UserManager.instance.ValidateNickname(nicknameInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("error_field_nickname"));

                    return;
                }

                if(string.IsNullOrEmpty(emailInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("error_field_empty"));

                    return;
                }

                if(!UserManager.instance.ValidateEmail(emailInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("error_field_email"));

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

                if(legalToggle.isOn == false)
                {
                    NotificationMenu.instance.Open(new Notification("error_field_legal"));

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.Register(nicknameInputField.text, emailInputField.text, passwordInputField.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("info_user_registered", () =>
                                {
                                    Close(() =>
                                    {
                                        callback?.Invoke(true);
                                    });
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
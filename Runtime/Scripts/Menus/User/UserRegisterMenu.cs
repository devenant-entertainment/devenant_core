using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserRegisterMenu : Menu<UserRegisterMenu>
    {
        [SerializeField] private Button closeButton;

        [SerializeField] private TMP_InputField nicknameInputfield;
        [SerializeField] private TMP_InputField emailInputfield;
        [SerializeField] private TMP_InputField passwordInputfield;
        [SerializeField] private Toggle acceptToggle;
        [SerializeField] private Button tosButton;

        [SerializeField] private Button registerButton;

        public override void Open(Action callback = null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                Close();
            });

            nicknameInputfield.text = string.Empty;
            emailInputfield.text = string.Empty;
            passwordInputfield.text = string.Empty;
            acceptToggle.isOn = false;

            registerButton.onClick.RemoveAllListeners();
            registerButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(nicknameInputfield.text))
                {
                    NotificationMenu.instance.Open(new NotificationMenu.Notification("user_empty_fields"));

                    return;
                }

                if(!UserManager.instance.ValidateNickname(nicknameInputfield.text))
                {
                    NotificationMenu.instance.Open(new NotificationMenu.Notification("invalid_nickname"));

                    return;
                }

                if(string.IsNullOrEmpty(emailInputfield.text))
                {
                    NotificationMenu.instance.Open(new NotificationMenu.Notification("user_empty_fields"));

                    return;
                }

                if(!UserManager.instance.ValidateEmail(emailInputfield.text))
                {
                    NotificationMenu.instance.Open(new NotificationMenu.Notification("user_invalid_email"));

                    return;
                }

                if(string.IsNullOrEmpty(passwordInputfield.text))
                {
                    NotificationMenu.instance.Open(new NotificationMenu.Notification("user_empty_fields"));

                    return;
                }

                if(!UserManager.instance.ValidatePassword(passwordInputfield.text))
                {
                    NotificationMenu.instance.Open(new NotificationMenu.Notification("user_invalid_password"));

                    return;
                }

                if(acceptToggle.isOn == false)
                {
                    NotificationMenu.instance.Open(new NotificationMenu.Notification("user_accept_off"));

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.Register(nicknameInputfield.text, emailInputfield.text, passwordInputfield.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("user_register_done", () =>
                                {
                                    Close(callback);
                                });
                            }
                            else
                            {
                                NotificationMenu.instance.Open(new NotificationMenu.Notification(response.message));
                            }    
                        });
                    });
                });
            });

            tosButton.onClick.RemoveAllListeners();
            tosButton.onClick.AddListener(() =>
            {
                UnityEngine.Application.OpenURL(Application.config.legalUrl);
            });

            base.Open();
        }
    }
}
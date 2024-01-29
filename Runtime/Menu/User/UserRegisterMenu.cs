using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserRegisterMenu : Menu<UserRegisterMenu>
    {
        [SerializeField] private TMP_InputField nicknameInputfield;
        [SerializeField] private TMP_InputField emailInputfield;
        [SerializeField] private TMP_InputField passwordInputfield;
        [SerializeField] private Toggle acceptToggle;
        [SerializeField] private Button tosButton;

        [SerializeField] private Button registerButton;

        [SerializeField] private Button closeButton;

        public void Open()
        {
            nicknameInputfield.text = string.Empty;
            emailInputfield.text = string.Empty;
            passwordInputfield.text = string.Empty;
            acceptToggle.isOn = false;

            registerButton.onClick.RemoveAllListeners();
            registerButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(nicknameInputfield.text))
                {
                    NotificationMenu.instance.Open("empty_fields");

                    return;
                }

                if(!FieldValidator.ValidateNickname(nicknameInputfield.text))
                {
                    NotificationMenu.instance.Open("invalid_nickname");

                    return;
                }

                if(string.IsNullOrEmpty(emailInputfield.text))
                {
                    NotificationMenu.instance.Open("empty_fields");

                    return;
                }

                if(!FieldValidator.ValidateEmail(emailInputfield.text))
                {
                    NotificationMenu.instance.Open("invalid_email");

                    return;
                }

                if(string.IsNullOrEmpty(passwordInputfield.text))
                {
                    NotificationMenu.instance.Open("empty_fields");

                    return;
                }

                if(!FieldValidator.ValidatePassword(passwordInputfield.text))
                {
                    NotificationMenu.instance.Open("invalid_password");

                    return;
                }

                if(acceptToggle.isOn == false)
                {
                    NotificationMenu.instance.Open("empty_accept_tos");

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.Register(nicknameInputfield.text, DataManager.instance.avatars[0].name, emailInputfield.text, passwordInputfield.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("register_done", () =>
                                {
                                    Close();
                                });
                            }
                            else
                            {
                                NotificationMenu.instance.Open(response.message);
                            }    
                        });
                    });
                });
            });

            tosButton.onClick.RemoveAllListeners();
            tosButton.onClick.AddListener(() =>
            {
                Application.OpenURL(DataManager.instance.applicationData.infoUrl);
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
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserUpdatePasswordMenu : Menu<UserUpdatePasswordMenu>
    {
        [SerializeField] private Button closeButton;

        [SerializeField] private TMP_InputField emailInputfield;
        [SerializeField] private TMP_InputField passwordInputfield;
        [SerializeField] private TMP_InputField codeInputfield;

        [SerializeField] private Button updateButton;

        public override void Open(Action callback = null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                Close();
            });

            emailInputfield.text = string.Empty;
            passwordInputfield.text = string.Empty;
            codeInputfield.text = string.Empty;

            updateButton.onClick.RemoveAllListeners();
            updateButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(emailInputfield.text))
                {
                    NotificationMenu.instance.Open("user_empty_fields");

                    return;
                }

                if(!UserManager.instance.ValidateEmail(emailInputfield.text))
                {
                    NotificationMenu.instance.Open("user_invalid_email");

                    return;
                }

                if(string.IsNullOrEmpty(passwordInputfield.text))
                {
                    NotificationMenu.instance.Open("user_empty_fields");

                    return;
                }

                if(!UserManager.instance.ValidatePassword(passwordInputfield.text))
                {
                    NotificationMenu.instance.Open("user_invalid_password");

                    return;
                }

                if(string.IsNullOrEmpty(codeInputfield.text))
                {
                    NotificationMenu.instance.Open("user_empty_fields");

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.UpdatePassword(passwordInputfield.text, codeInputfield.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("user_update_password_done", () =>
                                {
                                    Close(() =>
                                    {
                                        callback?.Invoke();
                                    });
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

            base.Open();
        }
    }
}

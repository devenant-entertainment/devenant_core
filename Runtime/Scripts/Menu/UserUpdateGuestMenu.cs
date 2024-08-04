using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserUpdateGuestMenu : Menu<UserUpdateGuestMenu>
    {
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;

        [SerializeField] private Button updateButton;

        [SerializeField] private Button closeButton;

        public override void Open(Action callback = null)
        {
            emailInputField.text = string.Empty;
            emailInputField.contentType = TMP_InputField.ContentType.EmailAddress;
            emailInputField.characterLimit = 256;

            passwordInputField.text = string.Empty;
            passwordInputField.contentType = TMP_InputField.ContentType.Password;

            updateButton.onClick.RemoveAllListeners();
            updateButton.onClick.AddListener(() =>
            {
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

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.UpdateGuest(emailInputField.text, passwordInputField.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("info_user_registered", () =>
                                {
                                    Close(() =>
                                    {
                                        callback?.Invoke();
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
                Close();
            });

            base.Open();
        }
    }
}

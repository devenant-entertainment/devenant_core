using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserUpdatePasswordMenu : Menu<UserUpdatePasswordMenu>
    {
        [SerializeField] private TMP_InputField codeInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private Button passwordShowButton;

        [SerializeField] private Button updateButton;
        
        [SerializeField] private Button closeButton;

        public override void Open(Action callback = null)
        {
            codeInputField.text = string.Empty;
            codeInputField.contentType = TMP_InputField.ContentType.Alphanumeric;
            codeInputField.characterLimit = 6;

            passwordInputField.text = string.Empty;
            passwordInputField.contentType = TMP_InputField.ContentType.Password;

            passwordShowButton.onClick.RemoveAllListeners();
            passwordShowButton.onClick.AddListener(() =>
            {
                passwordInputField.DeactivateInputField();
                passwordInputField.contentType = passwordInputField.contentType == TMP_InputField.ContentType.Password ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
                passwordInputField.ActivateInputField();
            });

            updateButton.onClick.RemoveAllListeners();
            updateButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(codeInputField.text))
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
                    UserManager.instance.UpdatePassword(codeInputField.text, passwordInputField.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("info_user_update_password", () =>
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

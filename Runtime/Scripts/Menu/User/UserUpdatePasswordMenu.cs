using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserUpdatePasswordMenu : Menu<UserUpdatePasswordMenu>
    {
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private TMP_InputField codeInputField;
        [Space]
        [SerializeField] private Button updateButton;
        [Space]
        [SerializeField] private Button closeButton;

        public override void Open(Action callback = null)
        {
            passwordInputField.text = string.Empty;
            passwordInputField.contentType = TMP_InputField.ContentType.Password;

            codeInputField.text = string.Empty;
            codeInputField.contentType = TMP_InputField.ContentType.Alphanumeric;
            codeInputField.characterLimit = 6;

            updateButton.onClick.RemoveAllListeners();
            updateButton.onClick.AddListener(() =>
            {
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

                if(string.IsNullOrEmpty(codeInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("user_empty_fields"));

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.UpdatePassword(passwordInputField.text, codeInputField.text, (Request.Response response) =>
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

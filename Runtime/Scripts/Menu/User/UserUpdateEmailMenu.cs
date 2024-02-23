using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserUpdateEmailMenu : Menu<UserUpdateEmailMenu>
    {
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField codeInputField;
        [Space]
        [SerializeField] private Button updateButton;
        [Space]
        [SerializeField] private Button closeButton;

        public override void Open(Action callback = null)
        {
            emailInputField.text = string.Empty;
            emailInputField.contentType = TMP_InputField.ContentType.EmailAddress;
            emailInputField.characterLimit = 256;

            codeInputField.text = string.Empty;
            codeInputField.contentType = TMP_InputField.ContentType.Alphanumeric;
            codeInputField.characterLimit = 6;

            updateButton.onClick.RemoveAllListeners();
            updateButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(emailInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("user_empty_fields"));

                    return;
                }

                if(!UserManager.instance.ValidateEmail(emailInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("user_invalid_email"));

                    return;
                }

                if(string.IsNullOrEmpty(codeInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("user_empty_fields"));

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.UpdateEmail(emailInputField.text, codeInputField.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("user_update_email_done", () =>
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
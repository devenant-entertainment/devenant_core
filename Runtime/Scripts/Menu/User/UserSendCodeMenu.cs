using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserSendCodeMenu : Menu<UserSendCodeMenu>
    {
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private Button acceptButton;
        [Space]
        [SerializeField] private Button closeButton;

        public void Open(Action<bool> callback = null)
        {
            emailInputField.text = UserManager.instance.user != null ? UserManager.instance.user.email : string.Empty;
            emailInputField.contentType = TMP_InputField.ContentType.EmailAddress;
            emailInputField.characterLimit = 256;

            acceptButton.onClick.RemoveAllListeners();
            acceptButton.onClick.AddListener(() =>
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

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.SendCode(emailInputField.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("user_code_done", () =>
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

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserSendCodeMenu : Menu<UserSendCodeMenu>
    {
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private Button sendButton;
        
        [SerializeField] private Button closeButton;

        public void Open(Action<bool> callback = null)
        {
            emailInputField.text = UserManager.instance.user != null ? UserManager.instance.user.email : string.Empty;
            emailInputField.contentType = TMP_InputField.ContentType.EmailAddress;
            emailInputField.characterLimit = 256;

            sendButton.onClick.RemoveAllListeners();
            sendButton.onClick.AddListener(() =>
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

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.SendCode(emailInputField.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("info_user_code", () =>
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

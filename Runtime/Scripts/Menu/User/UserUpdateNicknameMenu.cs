using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserUpdateNicknameMenu : Menu<UserUpdateNicknameMenu>
    {
        [SerializeField] private TMP_InputField nicknameInputField;
        [SerializeField] private TMP_InputField codeInputField;
        [Space]
        [SerializeField] private Button updateButton;
        [Space]
        [SerializeField] private Button closeButton;

        public override void Open(Action callback = null)
        {
            nicknameInputField.text = string.Empty;
            nicknameInputField.contentType = TMP_InputField.ContentType.Name;
            nicknameInputField.characterLimit = 32;

            codeInputField.text = string.Empty;
            codeInputField.contentType = TMP_InputField.ContentType.Alphanumeric;
            codeInputField.characterLimit = 6;

            updateButton.onClick.RemoveAllListeners();
            updateButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(nicknameInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("user_empty_fields"));

                    return;
                }

                if(!UserManager.instance.ValidateNickname(nicknameInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("user_invalid_nickname"));

                    return;
                }

                if(string.IsNullOrEmpty(codeInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("user_empty_fields"));

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.UpdateNickname(nicknameInputField.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("user_update_nickname_done", () =>
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

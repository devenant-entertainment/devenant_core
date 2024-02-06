using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserUpdateNicknameMenu : Menu<UserUpdateNicknameMenu>
    {
        [SerializeField] private Button closeButton;

        [SerializeField] private TMP_InputField nicknameInputfield;

        [SerializeField] private Button updateButton;

        public override void Open(Action callback = null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                Close();
            });

            nicknameInputfield.text = string.Empty;

            updateButton.onClick.RemoveAllListeners();
            updateButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(nicknameInputfield.text))
                {
                    NotificationMenu.instance.Open(new NotificationMenu.Notification("user_empty_fields"));

                    return;
                }

                if(!UserManager.instance.ValidateNickname(nicknameInputfield.text))
                {
                    NotificationMenu.instance.Open(new NotificationMenu.Notification("user_invalid_nickname"));

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.UpdateNickname(nicknameInputfield.text, (Request.Response response) =>
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
                                NotificationMenu.instance.Open(new NotificationMenu.Notification(response.message));
                            }
                        });
                    });
                });
            });

            base.Open();
        }
    }
}

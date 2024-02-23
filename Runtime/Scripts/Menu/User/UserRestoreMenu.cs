using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserRestoreMenu : Menu<UserRestoreMenu>
    {
        [SerializeField] private TMP_InputField codeInputField;
        [Space]
        [SerializeField] private Button restoreButton;
        [Space]
        [SerializeField] private Button closeButton;

        public void Open(Action<bool> callback = null)
        {
            codeInputField.text = string.Empty;
            codeInputField.contentType = TMP_InputField.ContentType.Alphanumeric;
            codeInputField.characterLimit = 6;

            restoreButton.onClick.RemoveAllListeners();
            restoreButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(codeInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("user_empty_fields"));

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.Restore(codeInputField.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                callback?.Invoke(true);
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

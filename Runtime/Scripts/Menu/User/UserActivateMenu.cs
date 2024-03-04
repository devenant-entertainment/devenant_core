using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserActivateMenu : Menu<UserActivateMenu>
    {
        [SerializeField] private TMP_InputField codeInputField;
        
        [SerializeField] private Button validateButton;
        
        [SerializeField] private Button closeButton;

        public void Open(Action<bool> callback)
        {
            codeInputField.text = string.Empty;
            codeInputField.contentType = TMP_InputField.ContentType.Alphanumeric;
            codeInputField.characterLimit = 6;

            validateButton.onClick.RemoveAllListeners();
            validateButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(codeInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("error_field_empty"));

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.Activate(codeInputField.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                Close(() =>
                                {
                                    callback?.Invoke(true);
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

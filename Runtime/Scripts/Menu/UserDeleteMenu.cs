using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserDeleteMenu : Menu<UserDeleteMenu>
    {
        [SerializeField] private TMP_InputField codeInputField;
        
        [SerializeField] private Button deleteButton;
        
        [SerializeField] private Button closeButton;

        public void Open(Action<bool> callback = null)
        {
            codeInputField.text = string.Empty;
            codeInputField.contentType = TMP_InputField.ContentType.Alphanumeric;
            codeInputField.characterLimit = 6;

            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(codeInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("error_field_empty"));

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.Delete(codeInputField.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("info_user_deleted", () =>
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
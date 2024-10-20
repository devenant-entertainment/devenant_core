using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserUpdateNicknameMenu : Menu<UserUpdateNicknameMenu>
    {
        [SerializeField] private TMP_InputField codeInputField;
        [SerializeField] private TMP_InputField nicknameInputField;
        
        [SerializeField] private Button updateButton;
        
        [SerializeField] private Button closeButton;

        public override void Open(Action callback = null)
        {
            codeInputField.text = string.Empty;
            codeInputField.contentType = TMP_InputField.ContentType.Alphanumeric;
            codeInputField.characterLimit = 6;

            nicknameInputField.text = string.Empty;
            nicknameInputField.contentType = TMP_InputField.ContentType.Standard;
            nicknameInputField.characterLimit = 32;

            updateButton.onClick.RemoveAllListeners();
            updateButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(codeInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("error_field_empty"));

                    return;
                }

                if(string.IsNullOrEmpty(nicknameInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("error_field_empty"));

                    return;
                }

                if(!UserManager.instance.ValidateNickname(nicknameInputField.text))
                {
                    NotificationMenu.instance.Open(new Notification("error_field_nickname"));

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.UpdateNickname(codeInputField.text, nicknameInputField.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("info_user_update_nickname", () =>
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

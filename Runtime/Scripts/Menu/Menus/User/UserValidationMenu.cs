using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserValidationMenu : Menu<UserValidationMenu>
    {
        [SerializeField] private Button closeButton;

        [SerializeField] private TMP_InputField codeInputfield;

        [SerializeField] private Button activateButton;

        public override void Open(Action callback)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                MessageMenu.instance.Open("exit_app", (bool response) =>
                {
                    if(response)
                    {
                        Application.Exit();
                    }
                });
            });

            codeInputfield.text = string.Empty;

            activateButton.onClick.RemoveAllListeners();
            activateButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(codeInputfield.text))
                {
                    NotificationMenu.instance.Open(new NotificationMenu.Notification("user_empty_fields"));

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.Validate(codeInputfield.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("user_validation_done", () =>
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

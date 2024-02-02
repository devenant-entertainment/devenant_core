using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserDeletionMenu : Menu<UserDeletionMenu>
    {
        [SerializeField] private Button closeButton;

        [SerializeField] private TMP_InputField emailInputfield;
        [SerializeField] private TMP_InputField codeInputfield;

        [SerializeField] private Button deleteButton;

        public override void Open(Action callback = null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                Close();
            });

            emailInputfield.text = string.Empty;
            codeInputfield.text = string.Empty;

            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(emailInputfield.text))
                {
                    NotificationMenu.instance.Open("user_empty_fields");

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.Delete(emailInputfield.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("user_delete_done", () =>
                                {
                                    UserManager.instance.Logout();
                                });
                            }
                            else
                            {
                                NotificationMenu.instance.Open(response.message);
                            }
                        });
                    });
                });
            });

            base.Open();
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserDeletionMenu : Menu<UserDeletionMenu>
    {
        [SerializeField] private GameObject infoHolder;

        [SerializeField] private GameObject emailHolder;
        [SerializeField] private TMP_InputField emailInputfield;

        [SerializeField] private GameObject codeHolder;
        [SerializeField] private TMP_InputField codeInputfield;

        [SerializeField] private Button deleteButton;
        [SerializeField] private Button closerButton;

        public override void Open(Action callback = null)
        {
            infoHolder.SetActive(true);

            emailHolder.SetActive(true);
            emailInputfield.text = string.Empty;

            codeHolder.SetActive(false);
            codeInputfield.text = string.Empty;

            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(emailInputfield.text))
                {
                    NotificationMenu.instance.Open("empty_fields");

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.DeleteGet(emailInputfield.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                NotificationMenu.instance.Open("delete_account_get_done");

                                infoHolder.SetActive(false);

                                emailHolder.SetActive(false);

                                codeHolder.SetActive(true);

                                deleteButton.onClick.RemoveAllListeners();
                                deleteButton.onClick.AddListener(() =>
                                {
                                    if(string.IsNullOrEmpty(codeInputfield.text))
                                    {
                                        NotificationMenu.instance.Open("empty_fields");

                                        return;
                                    }

                                    LoadingMenu.instance.Open(() =>
                                    {
                                        UserManager.instance.DeleteUpdate(codeInputfield.text, (Request.Response response) =>
                                        {
                                            LoadingMenu.instance.Close(() =>
                                            {
                                                if(response.success)
                                                {
                                                    MessageMenu.instance.Open("delete_account_update_done", () =>
                                                    {
                                                        UserManager.instance.Logout();

                                                        ApplicationManager.instance.Exit();
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
                            }
                            else
                            {
                                NotificationMenu.instance.Open(response.message);
                            }
                        });
                    });
                });
            });

            closerButton.onClick.RemoveAllListeners();
            closerButton.onClick.AddListener(() =>
            {
                Close();
            });

            base.Open();
        }
    }
}
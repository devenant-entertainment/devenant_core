using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserPasswordUpdateMenu : Menu<UserPasswordUpdateMenu>
    {
        [SerializeField] private GameObject infoHolder;

        [SerializeField] private GameObject emailHolder;
        [SerializeField] private TMP_InputField emailInputfield;
        [SerializeField] private GameObject codeHolder;
        [SerializeField] private TMP_InputField codeInputfield;
        [SerializeField] private GameObject passwordHolder;
        [SerializeField] private TMP_InputField passwordInputfield;

        [SerializeField] private Button updateButton;
        [SerializeField] private Button closerButton;

        public void Open(string email, Action callback = null)
        {
            infoHolder.SetActive(true);

            emailHolder.SetActive(true);
            emailInputfield.text = email;

            codeHolder.SetActive(false);
            codeInputfield.text = string.Empty;

            passwordHolder.SetActive(false);
            passwordInputfield.text = string.Empty;

            updateButton.onClick.RemoveAllListeners();
            updateButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(emailInputfield.text))
                {
                    NotificationMenu.instance.Open("empty_fields");

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.UpdatePasswordGet(emailInputfield.text , (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                NotificationMenu.instance.Open("update_password_get_done");

                                infoHolder.SetActive(false);

                                emailHolder.SetActive(false);

                                codeHolder.SetActive(true);

                                passwordHolder.SetActive(true);

                                updateButton.onClick.RemoveAllListeners();
                                updateButton.onClick.AddListener(() =>
                                {
                                    if(string.IsNullOrEmpty(codeInputfield.text))
                                    {
                                        NotificationMenu.instance.Open("empty_fields");

                                        return;
                                    }

                                    if(string.IsNullOrEmpty(passwordInputfield.text))
                                    {
                                        NotificationMenu.instance.Open("empty_fields");

                                        return;
                                    }

                                    if(!FieldValidator.ValidatePassword(passwordInputfield.text))
                                    {
                                        NotificationMenu.instance.Open("invalid_password");

                                        return;
                                    }

                                    LoadingMenu.instance.Open(() =>
                                    {
                                        UserManager.instance.UpdatePasswordUpdate(codeInputfield.text, emailInputfield.text, passwordInputfield.text, (Request.Response response) =>
                                        {
                                            LoadingMenu.instance.Close(() =>
                                            {
                                                if(response.success)
                                                {
                                                    MessageMenu.instance.Open("update_password_update_done", () =>
                                                    {
                                                        Close(() => 
                                                        {
                                                            callback?.Invoke();
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

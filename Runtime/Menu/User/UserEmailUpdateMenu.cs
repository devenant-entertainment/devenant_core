using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserEmailUpdateMenu : Menu<UserEmailUpdateMenu>
    {
        [SerializeField] private GameObject infoHolder;

        [SerializeField] private GameObject emailHolder;
        [SerializeField] private TMP_InputField emailInputfield;

        [SerializeField] private GameObject codeHolder;
        [SerializeField] private TMP_InputField codeInputfield;

        [SerializeField] private Button updateButton;
        [SerializeField] private Button closerButton;

        public override void Open(Action callback = null)
        {
            infoHolder.SetActive(true);

            emailHolder.SetActive(true);
            emailInputfield.text = string.Empty;

            codeHolder.SetActive(false);
            codeInputfield.text = string.Empty;

            updateButton.onClick.RemoveAllListeners();
            updateButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(emailInputfield.text))
                {
                    NotificationMenu.instance.Open("empty_fields");

                    return;
                }

                if(!FieldValidator.ValidateEmail(emailInputfield.text))
                {
                    NotificationMenu.instance.Open("invalid_email");

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.UpdateEmailGet(emailInputfield.text, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                NotificationMenu.instance.Open("update_email_get_done");

                                infoHolder.SetActive(false);

                                emailHolder.SetActive(false);

                                codeHolder.SetActive(true);

                                updateButton.onClick.RemoveAllListeners();
                                updateButton.onClick.AddListener(() =>
                                {
                                    if(string.IsNullOrEmpty(codeInputfield.text))
                                    {
                                        NotificationMenu.instance.Open("empty_fields");

                                        return;
                                    }

                                    LoadingMenu.instance.Open(() =>
                                    {
                                        UserManager.instance.UpdateEmailUpdate(codeInputfield.text, emailInputfield.text, (Request.Response response) =>
                                        {
                                            LoadingMenu.instance.Close(() =>
                                            {
                                                if(response.success)
                                                {
                                                    MessageMenu.instance.Open("update_email_update_done", () =>
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
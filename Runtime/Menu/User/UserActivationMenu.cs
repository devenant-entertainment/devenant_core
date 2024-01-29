using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserActivationMenu : Menu<UserActivationMenu>
    {
        [System.Serializable]
        private class ActivationCodeData
        {
            public string code;
        }

        [SerializeField] private GameObject infoHolder;

        [SerializeField] private GameObject codeHolder;
        [SerializeField] private TMP_InputField codeInputfield;

        [SerializeField] private Button activateButton;
        [SerializeField] private Button closerButton;

        public override void Open(Action callback)
        {
            infoHolder.SetActive(true);
            codeHolder.SetActive(false);
            codeInputfield.text = string.Empty;

            activateButton.onClick.RemoveAllListeners();
            activateButton.onClick.AddListener(() =>
            {
                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.ActivationGet((Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if (response.success)
                            {
                                NotificationMenu.instance.Open("activation_get_done");

                                infoHolder.SetActive(false);
                                codeHolder.SetActive(true);
                                codeInputfield.text = response.data == "null" ? string.Empty : JsonUtility.FromJson<ActivationCodeData>(response.data).code;

                                activateButton.onClick.RemoveAllListeners();
                                activateButton.onClick.AddListener(() =>
                                {
                                    if(string.IsNullOrEmpty(codeInputfield.text))
                                    {
                                        NotificationMenu.instance.Open("empty_fields");

                                        return;
                                    }

                                    LoadingMenu.instance.Open(() =>
                                    {
                                        UserManager.instance.ActivationUpdate(codeInputfield.text, (Request.Response response) =>
                                        {
                                            LoadingMenu.instance.Close(() =>
                                            {
                                                if(response.success)
                                                {
                                                    MessageMenu.instance.Open("activation_update_done", () => 
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
                MessageMenu.instance.Open("exit_app", (bool response) => 
                {
                    if(response)
                    {
                        ApplicationManager.instance.Exit();
                    }
                });
            });

            base.Open();
        }
    }
}

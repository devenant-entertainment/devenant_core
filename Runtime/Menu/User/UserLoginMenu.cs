using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserLoginMenu : Menu<UserLoginMenu>
    {
        [SerializeField] private Button closeButton;

        [SerializeField] private TMP_InputField emailInputfield;
        [SerializeField] private TMP_InputField passwordInputfield;
        [SerializeField] private Toggle rememberToggle;

        [SerializeField] private Button loginButton;
        [SerializeField] private Button updatePasswordButton;
        [SerializeField] private Button registerButton;

        public override void Open(Action callback = null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                MessageMenu.instance.Open("exit_app", (bool response) =>
                {
                    if(response)
                    {
                        ApplicationManager.instance.Exit();
                    }
                });
            });

            emailInputfield.text = string.Empty;
            passwordInputfield.text = string.Empty;
            rememberToggle.isOn = true;

            loginButton.onClick.RemoveAllListeners();
            loginButton.onClick.AddListener(() =>
            {
                if(string.IsNullOrEmpty(emailInputfield.text))
                {
                    NotificationMenu.instance.Open("empty_fields");

                    return;
                }

                if(string.IsNullOrEmpty(passwordInputfield.text))
                {
                    NotificationMenu.instance.Open("empty_fields");

                    return;
                }

                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.Login(emailInputfield.text, passwordInputfield.text, rememberToggle.isOn, (Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                Close(() => 
                                {
                                    callback?.Invoke();
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

            registerButton.onClick.RemoveAllListeners();
            registerButton.onClick.AddListener(() =>
            {
                UserRegisterMenu.instance.Open();
            });

            updatePasswordButton.onClick.RemoveAllListeners();
            updatePasswordButton.onClick.AddListener(() =>
            {
                UserPasswordUpdateMenu.instance.Open(string.Empty);
            });

            base.Open();
        }
    }
}

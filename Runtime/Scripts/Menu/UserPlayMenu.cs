using System;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserPlayMenu : Menu<UserPlayMenu>
    {
        [SerializeField] private Button loginButton;
        [SerializeField] private Button guestButton;

        [SerializeField] private Button settingsButton;

        public override void Open(Action callback = null)
        {
            loginButton.onClick.RemoveAllListeners();
            loginButton.onClick.AddListener(() =>
            {
                UserLoginMenu.instance.Open(() =>
                {
                    Close(() =>
                    {
                        callback?.Invoke();
                    });
                });
            });

            guestButton.onClick.RemoveAllListeners();
            guestButton.onClick.AddListener(() =>
            {
                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.LoginGuest((Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if (response.success)
                            {
                                Close(() =>
                                {
                                    callback?.Invoke();
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

            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(() =>
            {
                SettingsMenu.instance.Open();
            });

            base.Open();
        }
    }
}

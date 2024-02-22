using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserCodeMenu : Menu<UserCodeMenu>
    {
        [SerializeField] private Button closeButton;

        [SerializeField] private Button codeButton;

        public override void Open(Action callback = null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                Close();
            });

            codeButton.onClick.RemoveAllListeners();
            codeButton.onClick.AddListener(() =>
            {
                LoadingMenu.instance.Open(() =>
                {
                    UserManager.instance.Code((Request.Response response) =>
                    {
                        LoadingMenu.instance.Close(() =>
                        {
                            if(response.success)
                            {
                                MessageMenu.instance.Open("user_code_done", () =>
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

            base.Open(callback);
        }
    }
}

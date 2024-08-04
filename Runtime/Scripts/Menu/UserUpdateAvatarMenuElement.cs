using System;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserUpdateAvatarMenuElement : MonoBehaviour
    {
        [SerializeField] private Button selectButton;
        [SerializeField] private Image avatarImage;
        [SerializeField] private GameObject lockedIndicator;
        [SerializeField] private GameObject selectedIndicator;

        public void Setup(AvatarData avatar, Action callback)
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() =>
            {
                if(avatar.IsUnlocked())
                {
                    LoadingMenu.instance.Open(() =>
                    {
                        UserManager.instance.UpdateAvatar(avatar.name, (Request.Response response) =>
                        {
                            LoadingMenu.instance.Close(() =>
                            {
                                if(response.success)
                                {
                                    callback?.Invoke();
                                }
                                else
                                {
                                    NotificationMenu.instance.Open(new Notification(response.message));
                                }
                            });
                        });
                    });
                }
            });

            avatarImage.sprite = avatar.icon;

            lockedIndicator.SetActive(!avatar.IsUnlocked());

            selectedIndicator.SetActive(UserManager.instance.data.avatar == avatar.name);

        }
    }
}

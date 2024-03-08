using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class AvatarMenuElement : MonoBehaviour
    {
        [SerializeField] private Button selectButton;
        [SerializeField] private Image avatarImage;
        [SerializeField] private GameObject lockedIndicator;
        [SerializeField] private GameObject selectedIndicator;

        public void Setup(Avatar avatar, Action callback)
        {
            bool isLocked = IsLocked(avatar);

            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() =>
            {
                if(!isLocked)
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

            avatarImage.sprite = avatar.sprite;

            lockedIndicator.SetActive(isLocked);

            selectedIndicator.SetActive(UserManager.instance.user.avatar == avatar.name);

        }

        private bool IsLocked(Avatar avatar)
        {
            bool locked = false;

            if(!string.IsNullOrEmpty(avatar.purchase))
            {
                locked = true;

                foreach(Purchase purchase in PurchaseManager.instance.purchases)
                {
                    if(purchase.name == avatar.purchase && purchase.purchased)
                    {
                        locked = false;
                    }
                }
            }

            if(!string.IsNullOrEmpty(avatar.achievement))
            {
                locked = true;

                foreach(Achievement achievement in AchievementManager.instance.achievements)
                {
                    if(achievement.name == avatar.achievement && achievement.completed)
                    {
                        locked = false;
                    }
                }
            }

            return locked;
        }
    }
}

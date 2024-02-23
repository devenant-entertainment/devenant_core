using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserUpdateAvatarMenu : Menu<UserUpdateAvatarMenu>
    {
        [SerializeField] private RectTransform avatarHolder;
        [SerializeField] private GameObject avatarElement;
        [Space]
        [SerializeField] private Button closeButton;

        private Content avatarContent;

        public override void Open(Action callback = null)
        {
            GenerateAvatars();

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                Close();
            });

            base.Open();
        }

        private void GenerateAvatars()
        {
            avatarContent?.Clear();

            foreach(Avatar avatar in AvatarManager.instance.avatars)
            {
                bool locked = IsLocked(avatar);

                GameObject newAvatar = avatarContent.Create();

                newAvatar.transform.Find("Selected").gameObject.SetActive(UserManager.instance.user.avatar == avatar.id);

                newAvatar.transform.Find("AvatarImage").GetComponent<Image>().sprite = avatar.sprite;

                newAvatar.transform.Find("Locked").gameObject.SetActive(locked);

                newAvatar.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if(!locked)
                    {
                        LoadingMenu.instance.Open(() =>
                        {
                            UserManager.instance.UpdateAvatar(avatar.id, (Request.Response response) =>
                            {
                                LoadingMenu.instance.Close(() =>
                                {
                                    if(response.success)
                                    {
                                        GenerateAvatars();
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
            }

            bool IsLocked(Avatar avatar)
            {
                bool locked = false;

                if(!string.IsNullOrEmpty(avatar.purchase))
                {
                    locked = true;

                    foreach(Purchase purchase in PurchaseManager.instance.purchases)
                    {
                        if(purchase.id == avatar.purchase && purchase.purchased)
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
                        if(achievement.id == avatar.achievement && achievement.completed)
                        {
                            locked = false;
                        }
                    }
                }

                return locked;
            }
        }
    }
}

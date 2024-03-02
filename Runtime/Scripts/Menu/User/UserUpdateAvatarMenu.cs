using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserUpdateAvatarMenu : Menu<UserUpdateAvatarMenu>
    {
        [SerializeField] private RectTransform avatarHolder;
        [SerializeField] private GameObject avatarElement;
        
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

            avatarContent = new Content(avatarHolder, avatarElement);

            foreach(Avatar avatar in AvatarManager.instance.avatars)
            {
                bool isLocked = IsLocked(avatar);

                GameObject newAvatar = avatarContent.Create();

                newAvatar.transform.Find("Selected").gameObject.SetActive(UserManager.instance.user.avatar == avatar.name);

                newAvatar.transform.Find("AvatarImage").GetComponent<Image>().sprite = avatar.sprite;

                newAvatar.transform.Find("Locked").gameObject.SetActive(isLocked);

                newAvatar.GetComponent<Button>().onClick.AddListener(() =>
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
}

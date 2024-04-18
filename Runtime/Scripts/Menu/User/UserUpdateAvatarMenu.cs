using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class UserUpdateAvatarMenu : Menu<UserUpdateAvatarMenu>
    {
        [SerializeField] private RectTransform avatarHolder;
        [SerializeField] private AvatarMenuElement avatarElement;
        
        [SerializeField] private Button closeButton;

        private MenuContent avatarContent;

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

            avatarContent = new MenuContent(avatarHolder, avatarElement.gameObject);

            foreach(Avatar avatar in AvatarManager.instance.avatars.Get())
            {
                avatarContent.Create().GetComponent<AvatarMenuElement>().Setup(avatar, () =>
                {
                    GenerateAvatars();
                });
            }
        }
    }
}

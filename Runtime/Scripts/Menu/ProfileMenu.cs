using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class ProfileMenu : Menu<ProfileMenu>
    {
        [SerializeField] private Image avatarImage;
        [SerializeField] private Button updateAvatarButton;
        
        [SerializeField] private TextMeshProUGUI nicknameText;
        [SerializeField] private Button updateNicknameButton;
        
        [SerializeField] private TextMeshProUGUI emailText;
        [SerializeField] private Button updateEmailButton;
        
        [SerializeField] private TextMeshProUGUI passwordText;
        [SerializeField] private Button updatePasswordButton;
        
        [SerializeField] private Button achievementsButton;
        
        [SerializeField] private Button logoutButton;
        
        [SerializeField] private Button deleteButton;
        
        [SerializeField] private Button closeButton;

        public override void Open(Action callback = null)
        {
            UserManager.onUserUpdated += OnUserUpdated;

            OnUserUpdated(UserManager.instance.user);

            updateAvatarButton.onClick.RemoveAllListeners();
            updateAvatarButton.onClick.AddListener(() =>
            {
                UserUpdateAvatarMenu.instance.Open();
            });

            updateNicknameButton.onClick.RemoveAllListeners();
            updateNicknameButton.onClick.AddListener(() =>
            {
                UserSendCodeMenu.instance.Open((bool success) =>
                {
                    if(success)
                    {
                        UserUpdateNicknameMenu.instance.Open();
                    }
                });
            });

            updateEmailButton.onClick.RemoveAllListeners();
            updateEmailButton.onClick.AddListener(() =>
            {
                UserSendCodeMenu.instance.Open((bool success) =>
                {
                    if(success)
                    {
                        UserUpdateEmailMenu.instance.Open(() =>
                        {
                            UserManager.instance.Logout();

                            ApplicationManager.instance.Exit();
                        });
                    }
                });
            });

            updatePasswordButton.onClick.RemoveAllListeners();
            updatePasswordButton.onClick.AddListener(() =>
            {
                UserSendCodeMenu.instance.Open((bool success) =>
                {
                    if(success)
                    {
                        UserUpdatePasswordMenu.instance.Open();
                    }
                });
            });

            achievementsButton.onClick.RemoveAllListeners();
            achievementsButton.onClick.AddListener(() =>
            {
                AchievementMenu.instance.Open();
            });

            logoutButton.onClick.RemoveAllListeners();
            logoutButton.onClick.AddListener(() =>
            {
                MessageMenu.instance.Open("dialogue_logout", (bool success) =>
                {
                    if(success)
                    {
                        UserManager.instance.Logout();

                        ApplicationManager.instance.Exit();
                    }
                });
            });

            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() =>
            {
                UserSendCodeMenu.instance.Open((bool success) =>
                {
                    if(success)
                    {
                        UserDeleteMenu.instance.Open((bool success) =>
                        {
                            if(success)
                            {
                                UserManager.instance.Logout();

                                ApplicationManager.instance.Exit();
                            }
                        });
                    }
                });
            });

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                Close();
            });

            base.Open(callback);
        }

        public override void Close(Action callback = null)
        {
            UserManager.onUserUpdated -= OnUserUpdated;

            base.Close(callback);
        }

        private void OnUserUpdated(User user)
        {
            avatarImage.sprite = AvatarManager.instance.Get(user.avatar);

            nicknameText.text = user.nickname;  

            emailText.text = user.email;

            passwordText.text = "********";
        }
    }
}

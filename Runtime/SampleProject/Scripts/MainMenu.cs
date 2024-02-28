using UnityEngine;
using UnityEngine.UI;

namespace Devenant.Samples
{
    public class MainMenu : Menu<MainMenu>
    {
        [SerializeField] private Button userMenuButton;

        public override void Open(Action callback = null)
        {
            userMenuButton.onClick.RemoveAllListeners();
            userMenuButton.onClick.AddListener(() =>
            {
                UserMenu.instance.Open();
            });

            base.Open(callback);
        }
    }
}
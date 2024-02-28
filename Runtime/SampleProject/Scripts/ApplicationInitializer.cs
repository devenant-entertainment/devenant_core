using UnityEngine;

namespace Devenant.Samples
{
    public class ApplicationInitializer : MonoBehaviour
    {
        [SerializeField] private ApplicationData application;
        [SerializeField] private BackendData backend;
        [SerializeField] private PurchaseData[] purchases;
        [SerializeField] private AchievementData[] achievements;
        [SerializeField] private AvatarData[] avatars;

        private void Start()
        {
            //PlayerPrefs.DeleteAll();

            ApplicationManager.instance.Initialize(application, backend, purchases, achievements, avatars, () =>
            {
                MainMenu.instance.Open();
            });

            Destroy(gameObject);
        }
    }
}
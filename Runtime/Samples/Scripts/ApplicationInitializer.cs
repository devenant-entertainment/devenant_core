using UnityEngine;

namespace Devenant.Samples
{
    public class ApplicationInitializer : MonoBehaviour
    {
        [SerializeField] private ApplicationEnvironment environment;

        private void Start()
        {
            ApplicationManager.instance.Initialize(new Application(environment, string.Empty, string.Empty, string.Empty, new DatabaseEndpoints()
            {
                achievementGet = "",
                achievementSet = "",
                config = "",
                gameCreate = "",
                gameDelete = "",
                gameGet = "",
                gameLoad = "",
                gameSave = "",
                purchaseGet = "",
                purchaseSet = "",
                userCode = "",
                userDelete = "",
                userLogin = "",
                userRegister = "",
                userUpdateAvatar = "",
                userUpdateEmail = "",
                userUpdateNickname = "",
                userUpdatePassword = "",
                userValidate = "",
            }),
                new Purchase.Info[]
                {
                    new Purchase.Info("starting_pack", PurchaseType.Purchase)
                },
                new Achievement.Info[]
                {
                    new Achievement.Info("level_01", 1)
                }
            );

            Destroy(gameObject);
        }
    }
}
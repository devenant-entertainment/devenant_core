using Devenant;
using UnityEngine;

public class ApplicationInitializer : MonoBehaviour
{
    [SerializeField] private Devenant.Application.Config.Environment environment;
    [SerializeField] private string coreApiUrl;
    [SerializeField] private string gameApiUrl;
    [SerializeField] private string gameUrl;
    [SerializeField] private string legalUrl;
    [SerializeField] private string storeUrl;

    private void Start()
    {
        Devenant.Application.Initialize(new Devenant.Application.Config(environment, coreApiUrl, gameApiUrl, gameUrl, legalUrl, storeUrl), 
            new PurchaseManager.Purchase.Info[]
            {
                new PurchaseManager.Purchase.Info("starting_pack", UnityEngine.Purchasing.ProductType.Consumable)
            },
            new AchievementManager.Achievement.Info[]
            {
                new AchievementManager.Achievement.Info("initialize", 1)
            }
        );

        Destroy(gameObject);
    }
}

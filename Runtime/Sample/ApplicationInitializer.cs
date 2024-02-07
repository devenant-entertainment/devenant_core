using UnityEngine;

public class ApplicationInitializer : MonoBehaviour
{
    [SerializeField] private Devenant.Application.Config.Environment environment;
    [SerializeField] private string apiUrl;
    [SerializeField] private string gameUrl;
    [SerializeField] private string legalUrl;
    [SerializeField] private string storeUrl;

    private void Start()
    {
        Devenant.Application.Initialize(new Devenant.Application.Config(environment, apiUrl, gameUrl,legalUrl, storeUrl));

        Destroy(gameObject);
    }
}

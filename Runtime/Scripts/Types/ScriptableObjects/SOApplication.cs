using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "application", menuName = "Devenant/Core/Application", order = 0)]
    public class SOApplication : SOAsset
    {
        public ApplicationEnvironment environment;

        [Header("URLs")]
        public string gameUrl;
        public string legalUrl;
        public string supportUrl;

        [Header("Stores")]
        public string windowsStoreUrl;
        public string androidStoreUrl;
        public string iosStoreUrl;

        [Header("Config")]
        public float minInterfaceScale = 1.0f;
        public float maxInterfaceScale = 1.5f;
    }
}

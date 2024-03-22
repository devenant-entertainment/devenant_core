using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "application", menuName = "Devenant/Core/Application")]
    public class SOApplication : SOAsset
    {
        public ApplicationEnvironment environment;

        [Header("URLs")]
        [Required] public string gameUrl;
        [Required] public string legalUrl;
        [Required] public string supportUrl;

        [Header("Stores")]
        [Required] public string windowsStoreUrl;
        [Required] public string androidStoreUrl;
        [Required] public string iosStoreUrl;

        [Header("Config")]
        public float minInterfaceScale = 1.0f;
        public float maxInterfaceScale = 1.5f;
    }
}

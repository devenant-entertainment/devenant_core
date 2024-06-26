using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "application", menuName = "Devenant/Core/Application")]
    public class SOApplication : ScriptableObject
    {
        public ApplicationPlatform platform;
        public ApplicationEnvironment environment;
        public ApplicationMultiplayerMode multiplayerMode;

        [Header("URLs")]
        [Required] public string gameUrl;
        [Required] public string legalUrl;
        [Required] public string supportUrl;

        [Header("Stores")]
        [Required] public string steamUrl;
        [Required] public string microsoftStoreUrl;
        [Required] public string googlePlayUrl;
        [Required] public string appStoreUrl;

        [Header("Config")]
        public float minInterfaceScale = 1.0f;
        public float maxInterfaceScale = 1.5f;
    }
}

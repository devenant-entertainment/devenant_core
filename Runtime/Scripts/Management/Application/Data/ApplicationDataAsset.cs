using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "ApplicationData", menuName = "Devenant/Core/Application")]
    public class ApplicationDataAsset : ScriptableObject
    {
        [System.Serializable]
        public class StoreUrl
        {
            public RuntimePlatform platform;
            [Required] public string url;
        }

        public ApplicationEnvironment environment;
        public ApplicationMultiplayerMode multiplayerMode;

        [Header("URLs")]
        [Required] public string gameUrl;
        [Required] public string legalUrl;
        [Required] public string supportUrl;
        public StoreUrl[] storeUrls;

        [Header("Config")]
        public float minInterfaceScale = 1.0f;
        public float maxInterfaceScale = 1.5f;
    }
}

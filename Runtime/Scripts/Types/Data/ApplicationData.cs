using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "application", menuName = "Devenant/Core/Application", order = 0)]
    public class ApplicationData : ScriptableObject
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
    }
}

using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "ApplicationData", menuName = "Devenant/Application Data")]
    public class ApplicationData : ScriptableObject
    {
        [Tooltip("Base URL of the api")]
        [SerializeField] private string apiUrl;
        [Tooltip("Link to main game web.")]
        [SerializeField] private string gameUrl;
        [Tooltip("Link to legal portal web.")]
        [SerializeField] private string legalUrl;

        [Header("Stores")]
        [SerializeField] private string windowsStoreUrl;
        [SerializeField] private string androidStoreUrl;
        [SerializeField] private string iosStoreUrl;

        public string GetApiUrl()
        {
            return apiUrl;
        }

        public string GetGameUrl()
        {
            return gameUrl;
        }

        public string GetLegalUrl()
        {
            return legalUrl;
        }

        public string GetStoreUrl()
        {
            switch(Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:

                    return windowsStoreUrl;

                case RuntimePlatform.Android:

                    return androidStoreUrl;

                case RuntimePlatform.IPhonePlayer:

                    return iosStoreUrl;

                default:

                    return gameUrl;
            }
        }
    }
}

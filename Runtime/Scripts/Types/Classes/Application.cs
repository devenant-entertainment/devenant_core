namespace Devenant
{
    public enum ApplicationEnvironment
    {
        Production,
        Development
    }

    public class Application
    {
        public readonly ApplicationEnvironment environment;

        public readonly string gameUrl;
        public readonly string legalUrl;
        public readonly string supportUrl;

        private readonly string windowsStoreUrl;
        private readonly string androidStoreUrl;
        private readonly string iosStoreUrl;

        public Application(ApplicationData data)
        {
            environment = data.environment;

            gameUrl = data.gameUrl;
            legalUrl = data.legalUrl;
            supportUrl = data.supportUrl;

            windowsStoreUrl = data.windowsStoreUrl;
            androidStoreUrl = data.androidStoreUrl;
            iosStoreUrl = data.iosStoreUrl;
        }

        public string GetStoreUrl()
        {
            switch(UnityEngine.Application.platform)
            {
                default:

                    return windowsStoreUrl;

                case UnityEngine.RuntimePlatform.Android:

                    return androidStoreUrl;

                case UnityEngine.RuntimePlatform.IPhonePlayer:

                    return iosStoreUrl;
            }
        }
    }
}

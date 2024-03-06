namespace Devenant
{
    public enum ApplicationPlatform
    {
        Steam,
        Android,
        IOS,
    }

    public enum ApplicationEnvironment
    {
        Production,
        Development
    }

    public class Application
    {
        public readonly ApplicationPlatform platform;
        public readonly ApplicationEnvironment environment;

        public readonly string gameUrl;
        public readonly string legalUrl;
        public readonly string supportUrl;

        public readonly string storeUrl;

        public readonly float minInterfaceScale;
        public readonly float maxInterfaceScale;

        public Application(ApplicationData data)
        {
            environment = data.environment;

            gameUrl = data.gameUrl;
            legalUrl = data.legalUrl;
            supportUrl = data.supportUrl;

            minInterfaceScale = data.minInterfaceScale;
            maxInterfaceScale = data.maxInterfaceScale;

            switch(UnityEngine.Application.platform)
            {
                default:

                    platform = ApplicationPlatform.Steam;

                    storeUrl = data.windowsStoreUrl;

                    break;

                case UnityEngine.RuntimePlatform.Android:

                    platform = ApplicationPlatform.Android;

                    storeUrl = data.androidStoreUrl;

                    break;

                case UnityEngine.RuntimePlatform.IPhonePlayer:

                    platform = ApplicationPlatform.IOS;

                    storeUrl = data.iosStoreUrl;

                    break;
            }
        }
    }
}

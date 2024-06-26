namespace Devenant
{
    public enum ApplicationPlatform
    {
        Editor,
        Steam,
        MicrosoftStore,
        GooglePlay,
        AppStore
    }

    public enum ApplicationEnvironment
    {
        Production,
        Development
    }

    public enum ApplicationMultiplayerMode
    {
        Local,
        Online
    }

    public enum ApplicationStatus
    {
        Active,
        Inactive
    }

    public class Application
    {
        public readonly ApplicationPlatform platform;
        public readonly ApplicationEnvironment environment;
        public readonly ApplicationMultiplayerMode multiplayerMode;

        public readonly string gameUrl;
        public readonly string legalUrl;
        public readonly string supportUrl;

        public readonly string storeUrl;

        public readonly float minInterfaceScale;
        public readonly float maxInterfaceScale;

        public Application(SOApplication data)
        {
            platform = data.platform;
            environment = data.environment;
            multiplayerMode = data.multiplayerMode;

            gameUrl = data.gameUrl;
            legalUrl = data.legalUrl;
            supportUrl = data.supportUrl;

            switch(platform)
            {
                case ApplicationPlatform.Steam:

                    storeUrl = data.steamUrl;

                    break;

                case ApplicationPlatform.MicrosoftStore:

                    storeUrl = data.microsoftStoreUrl;

                    break;

                case ApplicationPlatform.GooglePlay:

                    storeUrl = data.googlePlayUrl;

                    break;

                case ApplicationPlatform.AppStore:

                    storeUrl = data.appStoreUrl;

                    break;
            }

            minInterfaceScale = data.minInterfaceScale;
            maxInterfaceScale = data.maxInterfaceScale;
        }
    }
}

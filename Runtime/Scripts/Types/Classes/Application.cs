namespace Devenant
{
    public enum ApplicationEnvironment
    {
        Production,
        Development
    }

    public struct DatabaseEndpoints
    {
        public string achievementGet;
        public string achievementSet;
        public string config;
        public string gameCreate;
        public string gameDelete;
        public string gameGet;
        public string gameLoad;
        public string gameSave;
        public string purchaseGet;
        public string purchaseSet;
        public string userCode;
        public string userDelete;
        public string userLogin;
        public string userRegister;
        public string userUpdateAvatar;
        public string userUpdateEmail;
        public string userUpdateNickname;
        public string userUpdatePassword;
        public string userValidate;
    }

    public class Application
    {
        public readonly ApplicationEnvironment environment;

        public readonly DatabaseEndpoints endpoints;

        public readonly string gameUrl;
        public readonly string legalUrl;
        public readonly string storeUrl;

        public Application(ApplicationEnvironment environment, string gameUrl, string legalUrl, string storeUrl, DatabaseEndpoints endpoints)
        {
            this.environment = environment;
            this.gameUrl = gameUrl;
            this.legalUrl = legalUrl;
            this.storeUrl = storeUrl;
            this.endpoints = endpoints;
        }
    }
}

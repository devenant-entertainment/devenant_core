namespace Devenant
{
    public class Backend 
    {
        public readonly string achievementGet;
        public readonly string achievementSet;
        public readonly string configuration;
        public readonly string gameCreate;
        public readonly string gameDelete;
        public readonly string gameGet;
        public readonly string gameLoad;
        public readonly string gameSave;
        public readonly string purchaseGet;
        public readonly string purchaseSet;
        public readonly string userActivate;
        public readonly string userDelete;
        public readonly string userLogin;
        public readonly string userRegister;
        public readonly string userSendCode;
        public readonly string userUpdateAvatar;
        public readonly string userUpdateEmail;
        public readonly string userUpdateNickname;
        public readonly string userUpdatePassword;

        public Backend(BackendData data)
        {
            achievementGet = data.achievementGet;
            achievementSet = data.achievementSet;
            configuration = data.configuration;
            gameCreate = data.gameCreate;
            gameDelete = data.gameDelete;
            gameGet = data.gameGet;
            gameLoad = data.gameLoad;
            gameSave = data.gameSave;
            purchaseGet = data.purchaseGet;
            purchaseSet = data.purchaseSet;
            userActivate = data.userActivate;
            userDelete = data.userDelete;
            userLogin = data.userLogin;
            userRegister = data.userRegister;
            userSendCode = data.userSendCode;
            userUpdateAvatar = data.userUpdateAvatar;
            userUpdateEmail = data.userUpdateEmail;
            userUpdateNickname = data.userUpdateNickname;
            userUpdatePassword = data.userUpdatePassword;
        }
    }
}

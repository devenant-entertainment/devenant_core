namespace Devenant
{
    public class Backend 
    {
        public readonly string achievementGet;
        public readonly string achievementSet;
        public readonly string config;
        public readonly string gameCreate;
        public readonly string gameDelete;
        public readonly string gameGet;
        public readonly string gameLoad;
        public readonly string gameSave;
        public readonly string purchaseGet;
        public readonly string purchaseSet;
        public readonly string userDelete;
        public readonly string userLogin;
        public readonly string userRegister;
        public readonly string userRestore;
        public readonly string userSendCode;
        public readonly string userUpdateAvatar;
        public readonly string userUpdateEmail;
        public readonly string userUpdateNickname;
        public readonly string userUpdatePassword;
        public readonly string userValidate;

        public Backend(BackendData data)
        {
            achievementGet = data.achievementGet;
            achievementSet = data.achievementSet;
            config = data.config;
            gameCreate = data.gameCreate;
            gameDelete = data.gameDelete;
            gameGet = data.gameGet;
            gameLoad = data.gameLoad;
            gameSave = data.gameSave;
            purchaseGet = data.purchaseGet;
            purchaseSet = data.purchaseSet;
            userDelete = data.userDelete;
            userLogin = data.userLogin;
            userRegister = data.userRegister;
            userRestore = data.userRestore;
            userSendCode = data.userSendCode;
            userUpdateAvatar = data.userUpdateAvatar;
            userUpdateEmail = data.userUpdateEmail;
            userUpdateNickname = data.userUpdateNickname;
            userUpdatePassword = data.userUpdatePassword;
            userValidate = data.userValidate;
        }
    }
}

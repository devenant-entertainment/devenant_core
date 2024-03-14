namespace Devenant
{
    public class Backend 
    {
        public readonly string achievementGet;
        public readonly string achievementSet;
        public readonly string configuration;
        public readonly string dataDelete;
        public readonly string dataGet;
        public readonly string dataLoad;
        public readonly string dataSave;
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

        public Backend(SOBackend data)
        {
            achievementGet = data.achievementGet;
            achievementSet = data.achievementSet;
            configuration = data.configuration;
            dataDelete = data.gameDelete;
            dataGet = data.gameGet;
            dataLoad = data.gameLoad;
            dataSave = data.gameSave;
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

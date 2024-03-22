    namespace Devenant
{
    public class Backend
    {
        public readonly string achievementGet;
        public readonly string achievementSet;

        public readonly string purchaseGet;
        public readonly string purchaseSet;

        public readonly string status;

        public readonly string storageDelete;
        public readonly string storageGet;
        public readonly string storageLoad;
        public readonly string storageSave;

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

            purchaseGet = data.purchaseGet;
            purchaseSet = data.purchaseSet;

            status = data.status;

            storageDelete = data.storageDelete;
            storageGet = data.storageGet;
            storageLoad = data.storageLoad;
            storageSave = data.storageSave;

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

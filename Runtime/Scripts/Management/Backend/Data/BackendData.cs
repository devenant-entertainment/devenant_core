    namespace Devenant
{
    public class BackendData
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
        public readonly string userLoginGuest;
        public readonly string userRegister;
        public readonly string userSendCode;
        public readonly string userUpdateAvatar;
        public readonly string userUpdateEmail;
        public readonly string userUpdateGuest;
        public readonly string userUpdateNickname;
        public readonly string userUpdatePassword;

        public BackendData(BackendDataAsset data)
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
            userLoginGuest = data.userLoginGuest;
            userRegister = data.userRegister;
            userSendCode = data.userSendCode;
            userUpdateAvatar = data.userUpdateAvatar;
            userUpdateEmail = data.userUpdateEmail;
            userUpdateGuest = data.userUpdateGuest;
            userUpdateNickname = data.userUpdateNickname;
            userUpdatePassword = data.userUpdatePassword;
        }
    }
}

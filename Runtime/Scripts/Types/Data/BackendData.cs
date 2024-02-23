using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName ="backend", menuName = "Devenant/Core/Backend", order = 1)]
    public class BackendData : ScriptableObject
    {
        [Header("Achievement")]
        public string achievementGet;
        public string achievementSet;

        [Header("Config")]
        public string config;

        [Header("Game")]
        public string gameCreate;
        public string gameDelete;
        public string gameGet;
        public string gameLoad;
        public string gameSave;

        [Header("Purchase")]
        public string purchaseGet;
        public string purchaseSet;

        [Header("User")]
        public string userDelete;
        public string userLogin;
        public string userRegister;
        public string userRestore;
        public string userSendCode;
        public string userUpdateAvatar;
        public string userUpdateEmail;
        public string userUpdateNickname;
        public string userUpdatePassword;
        public string userValidate;
    }
}

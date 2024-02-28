using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName ="backend", menuName = "Devenant/Core/Backend", order = 1)]
    public class BackendData : ScriptableObject
    {
        [Header("Achievement")]
        [TextArea(1, 2)] public string achievementGet;
        [TextArea(1, 2)] public string achievementSet;

        [Header("Config")]
        [TextArea(1, 2)] public string configuration;

        [Header("Game")]
        [TextArea(1, 2)] public string gameCreate;
        [TextArea(1, 2)] public string gameDelete;
        [TextArea(1, 2)] public string gameGet;
        [TextArea(1, 2)] public string gameLoad;
        [TextArea(1, 2)] public string gameSave;

        [Header("Purchase")]
        [TextArea(1, 2)] public string purchaseGet;
        [TextArea(1, 2)] public string purchaseSet;

        [Header("User")]
        [TextArea(1, 2)] public string userActivate;
        [TextArea(1, 2)] public string userDelete;
        [TextArea(1, 2)] public string userLogin;
        [TextArea(1, 2)] public string userRegister;
        [TextArea(1, 2)] public string userSendCode;
        [TextArea(1, 2)] public string userUpdateAvatar;
        [TextArea(1, 2)] public string userUpdateEmail;
        [TextArea(1, 2)] public string userUpdateNickname;
        [TextArea(1, 2)] public string userUpdatePassword;
    }
}

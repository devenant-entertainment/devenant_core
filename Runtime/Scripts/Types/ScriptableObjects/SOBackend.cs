using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName ="backend", menuName = "Devenant/Core/Backend", order = 1)]
    public class SOBackend : SOAsset
    {
        [Header("Achievement")]
        [Required][TextArea(1, 2)] public string achievementGet;
        [Required][TextArea(1, 2)] public string achievementSet;

        [Header("Config")]
        [Required][TextArea(1, 2)] public string configuration;

        [Header("Game")]
        [Required][TextArea(1, 2)] public string gameDelete;
        [Required][TextArea(1, 2)] public string gameGet;
        [Required][TextArea(1, 2)] public string gameLoad;
        [TextArea(1, 2)] public string gameSave;

        [Header("Purchase")]
        [Required][TextArea(1, 2)] public string purchaseGet;
        [Required][TextArea(1, 2)] public string purchaseSet;

        [Header("User")]
        [Required][TextArea(1, 2)] public string userActivate;
        [Required][TextArea(1, 2)] public string userDelete;
        [Required][TextArea(1, 2)] public string userLogin;
        [Required][TextArea(1, 2)] public string userRegister;
        [Required][TextArea(1, 2)] public string userSendCode;
        [Required][TextArea(1, 2)] public string userUpdateAvatar;
        [Required][TextArea(1, 2)] public string userUpdateEmail;
        [Required][TextArea(1, 2)] public string userUpdateNickname;
        [Required][TextArea(1, 2)] public string userUpdatePassword;
    }
}

using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "backend", menuName = "Devenant/Core/Backend", order = 1)]
    public class SOBackend : SOAsset
    {
        [Header("Achievement")]
        [Required] public string achievementGet;
        [Required] public string achievementSet;

        [Header("Config")]
        [Required] public string configuration;

        [Header("Data")]
        [Required] public string dataDelete;
        [Required] public string dataGet;
        [Required] public string dataLoad;
        [Required] public string dataSave;

        [Header("Purchase")]
        [Required] public string purchaseGet;
        [Required] public string purchaseSet;

        [Header("User")]
        [Required] public string userActivate;
        [Required] public string userDelete;
        [Required] public string userLogin;
        [Required] public string userRegister;
        [Required] public string userSendCode;
        [Required] public string userUpdateAvatar;
        [Required] public string userUpdateEmail;
        [Required] public string userUpdateNickname;
        [Required] public string userUpdatePassword;
    }
}

using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "backend", menuName = "Devenant/Core/Backend")]
    public class SOBackend : ScriptableObject
    {
        [Header("Achievement")]
        [Required] public string achievementGet;
        [Required] public string achievementSet;

        [Header("Purchase")]
        [Required] public string purchaseGet;
        [Required] public string purchaseSet;

        [Header("Status")]
        [Required] public string status;

        [Header("Storage")]
        [Required] public string storageDelete;
        [Required] public string storageGet;
        [Required] public string storageLoad;
        [Required] public string storageSave;

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

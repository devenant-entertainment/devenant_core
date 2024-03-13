using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "avatar_", menuName = "Devenant/Core/Avatar", order = 22)]
    public class SOAvatar : SOUnlockable
    {
        [Required] public Sprite icon;
    }
}

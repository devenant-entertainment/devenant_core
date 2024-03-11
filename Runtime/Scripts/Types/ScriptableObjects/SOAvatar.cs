using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "avatar_", menuName = "Devenant/Core/Avatar", order = 22)]
    public class SOAvatar : ScriptableObject
    {
        public Sprite sprite;
        public SOPurchase purchase;
        public SOAchievement achievement;
    }
}

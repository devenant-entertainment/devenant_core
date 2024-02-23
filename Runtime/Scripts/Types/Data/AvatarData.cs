using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "avatar_", menuName = "Devenant/Core/Avatar", order = 22)]
    public class AvatarData : ScriptableObject
    {
        public Sprite sprite;
        public PurchaseData purchase;
        public AchievementData achievement;
    }
}

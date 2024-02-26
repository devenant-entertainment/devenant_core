using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "achievement_", menuName = "Devenant/Core/Achievement", order = 21)]
    public class AchievementData : ScriptableObject
    {
        public Sprite icon;
        public int maxValue = 1;
    }
}

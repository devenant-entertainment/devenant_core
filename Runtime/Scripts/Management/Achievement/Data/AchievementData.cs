using UnityEngine;

namespace Devenant
{
    public class AchievementData : EntityData
    {
        public readonly int maxValue;

        public int value;

        public bool completed { get { return value == maxValue; } }

        public AchievementData(AchievementAsset achievement) : base(achievement)
        {
            maxValue = achievement.maxValue;
        }
    }
}

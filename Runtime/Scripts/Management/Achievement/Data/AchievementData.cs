using UnityEngine;

namespace Devenant
{
    public class AchievementData : AssetData
    {
        public readonly int maxValue;

        public int value;

        public bool completed { get { return value == maxValue; } }

        public AchievementData(AchievementDataAsset achievement) : base(achievement)
        {
            maxValue = achievement.maxValue;
        }
    }
}

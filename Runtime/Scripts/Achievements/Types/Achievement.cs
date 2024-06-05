using UnityEngine;

namespace Devenant
{
    public class Achievement : Asset
    {
        public readonly int maxValue;

        public int value;

        public bool completed { get { return value == maxValue; } }

        public Achievement(string name, Sprite icon, int maxValue) : base (name, icon)
        {
            this.maxValue = maxValue;
        }

        public Achievement(SOAchievement achievement) : base(achievement)
        {
            maxValue = achievement.maxValue;
        }
    }
}

using UnityEngine;

namespace Devenant
{
    public class Achievement
    {
        public readonly string name;
        public readonly Sprite icon;
        public readonly int maxValue;

        public int value;

        public Achievement(string name, Sprite icon, int maxValue)
        {
            this.name = name;
            this.icon = icon;
            this.maxValue = maxValue;
        }

        public bool IsCompleted()
        {
            return value == maxValue;
        }
    }
}

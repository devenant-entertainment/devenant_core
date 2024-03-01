using UnityEngine;

namespace Devenant
{
    public class Achievement
    {
        public readonly string name;
        public readonly Sprite icon;
        public readonly int maxValue;

        public int value;

        public bool completed
        {
            get 
            {
                return value == maxValue; 
            }
        }

        public Achievement(string name, Sprite icon, int maxValue, int value)
        {
            this.name = name;
            this.icon = icon;
            this.maxValue = maxValue;
            this.value = value;
        }
    }
}

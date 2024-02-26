using UnityEngine;

namespace Devenant
{
    public class Achievement
    {
        public readonly string id;
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

        public Achievement(string id, Sprite icon, int maxValue, int value)
        {
            this.id = id;
            this.icon = icon;
            this.maxValue = maxValue;
            this.value = value;
        }
    }
}

using UnityEngine;

namespace Devenant
{
    public class Achievement : Asset
    {
        public readonly int maxValue;

        public int value;

        public Achievement(string name, Sprite icon, int maxValue) : base (name, icon)
        {
            this.maxValue = maxValue;
        }

        public bool IsCompleted()
        {
            return value == maxValue;
        }
    }
}

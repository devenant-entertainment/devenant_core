using UnityEngine;

namespace Devenant
{
    public class Avatar : Unlockable
    {
        public readonly string name;
        public readonly Sprite icon;

        public Avatar(string name, Sprite icon, Achievement achievement, Purchase purchase) : base(achievement, purchase)
        {
            this.name = name;
            this.icon = icon;
        }
    }
}

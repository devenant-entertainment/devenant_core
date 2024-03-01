using UnityEngine;

namespace Devenant
{
    public class Avatar
    {
        public readonly string name;
        public readonly Sprite sprite;
        public readonly string purchase;
        public readonly string achievement;

        public Avatar (string name, Sprite sprite, string purchase, string achievement)
        {
            this.name = name;
            this.sprite = sprite;
            this.purchase = purchase;
            this.achievement = achievement;
        }
    }
}

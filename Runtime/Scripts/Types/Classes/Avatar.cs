using UnityEngine;

namespace Devenant
{
    public class Avatar
    {
        public readonly string id;
        public readonly Sprite sprite;
        public readonly string purchase;
        public readonly string achievement;

        public Avatar (string id, Sprite sprite, string purchase, string achievement)
        {
            this.id = id;
            this.sprite = sprite;
            this.purchase = purchase;
            this.achievement = achievement;
        }
    }
}

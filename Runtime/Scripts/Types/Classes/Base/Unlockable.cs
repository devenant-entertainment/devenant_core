using UnityEngine;

namespace Devenant
{
    public abstract class Unlockable : Asset
    {
        public readonly Achievement achievement;
        public readonly Purchase purchase;

        public Unlockable(string name, Sprite icon, Achievement achievement, Purchase purchase) : base (name, icon)
        {
            this.achievement = achievement;
            this.purchase = purchase;
        }

        public bool IsUnlocked()
        {
            bool result = true;

            if(achievement != null)
            {
                result = achievement.IsCompleted();
            }

            if(purchase != null && result)
            {
                result = purchase.purchased;
            }

            return result;
        }
    }
}

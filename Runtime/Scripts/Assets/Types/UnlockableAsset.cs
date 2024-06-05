using UnityEngine;

namespace Devenant
{
    public abstract class UnlockableAsset : Asset
    {
        public readonly Achievement achievement;
        public readonly Purchase purchase;

        public UnlockableAsset(string name, Sprite icon, Achievement achievement, Purchase purchase) : base (name, icon)
        {
            this.achievement = achievement;
            this.purchase = purchase;
        }

        public UnlockableAsset(SOUnlockableAsset unlockable) : base(unlockable)
        {
            if(unlockable.achievement != null)
            {
                achievement = AchievementManager.instance.achievements.Get(unlockable.achievement.name);
            }

            if(unlockable.purchase != null)
            {
                purchase = PurchaseManager.instance.purchases.Get(unlockable.purchase.name);
            }
        }

        public bool IsUnlocked()
        {
            bool result = true;

            if(achievement != null)
            {
                result = achievement.completed;
            }

            if(purchase != null && result)
            {
                result = purchase.purchased;
            }

            return result;
        }
    }
}

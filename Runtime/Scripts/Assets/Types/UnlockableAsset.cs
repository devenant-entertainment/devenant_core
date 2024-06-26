using System.Linq;
using UnityEngine;

namespace Devenant
{
    public abstract class UnlockableAsset : Asset
    {
        public readonly Achievement achievement;
        public readonly Product product;

        public UnlockableAsset(string name, Sprite icon, Achievement achievement, Product product) : base (name, icon)
        {
            this.achievement = achievement;
            this.product = product;
        }

        public UnlockableAsset(SOUnlockableAsset unlockable) : base(unlockable)
        {
            if(unlockable.achievement != null)
            {
                achievement = AchievementManager.instance.achievements.Get(unlockable.achievement.name);
            }

            if(unlockable.purchase != null)
            {
                product = PurchaseManager.instance.products.Get(unlockable.purchase.name);
            }
        }

        public bool IsUnlocked()
        {
            bool result = true;

            if(achievement != null)
            {
                result = achievement.completed;
            }

            if(product != null && result)
            {
                result = PurchaseManager.instance.storeProducts.ToList().Find((x)=>x.product == product).value;
            }

            return result;
        }
    }
}

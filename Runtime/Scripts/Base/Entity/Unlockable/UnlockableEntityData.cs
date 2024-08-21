using System.Linq;
using UnityEngine;

namespace Devenant
{
    public abstract class UnlockableEntityData : EntityData
    {
        public readonly AchievementData achievement;
        public readonly ProductData product;

        public UnlockableEntityData(UnlockableEntityAsset unlockable) : base(unlockable)
        {
            if(unlockable.achievement != null)
            {
                achievement = AchievementManager.instance.achievements.Get(unlockable.achievement.name);
            }

            if(unlockable.purchase != null)
            {
                product = StoreManager.instance.products.Get(unlockable.purchase.name);
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
                result = StoreManager.instance.storeProducts.ToList().Find((x)=>x.product == product).value;
            }

            return result;
        }
    }
}

namespace Devenant
{
    public abstract class UnlockableDataController<T, D> : AssetDataController<T, D> where T : Unlockable where D : SOUnlockable
    {
        protected Achievement GetAchievement(D unlockable)
        {
            return unlockable.achievement != null ? AchievementManager.instance.Get(unlockable.achievement.name) : null;
        }

        protected Purchase GetPurchase(D unlockable)
        {
            return unlockable.purchase != null ? PurchaseManager.instance.Get(unlockable.purchase.name) : null;
        }
    }
}

namespace Devenant
{
    public abstract class Unlockable
    {
        public readonly Achievement achievement;
        public readonly Purchase purchase;

        public Unlockable(Achievement achievement, Purchase purchase)
        {
            this.achievement = achievement;
            this.purchase = purchase;
        }

        public bool IsUnlocked()
        {
            bool result = true;

            if(achievement != null)
            {
                result = AchievementManager.instance.achievements.Get(achievement.name).IsCompleted();
            }

            if(purchase != null && result)
            {
                result = PurchaseManager.instance.purchases.Get(purchase.name).purchased;
            }

            return result;
        }
    }
}

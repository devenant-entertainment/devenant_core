namespace Devenant
{
    public class DataManager : Singleton<DataManager>
    {
        public AchievementDataController achievementDataController = new AchievementDataController();
        public PurchaseDataController purchaseDataController = new PurchaseDataController();
        public AvatarDataController avatarDataController = new AvatarDataController();
    }
}

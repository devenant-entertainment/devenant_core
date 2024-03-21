using System.Collections.Generic;

namespace Devenant
{
    public class AchievementDataController : AssetDataController<Achievement, SOAchievement>
    {
        protected override List<Achievement> NormalizeData(SOAchievement[] data)
        {
            List<Achievement> result = new List<Achievement>();

            foreach(SOAchievement achievement in data)
            {
                result.Add(new Achievement(achievement.name, achievement.icon, achievement.maxValue));
            }

            return result;
        }
    }
}

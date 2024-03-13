using System.Collections.Generic;

namespace Devenant
{
    public class AchievementDataContent : DataContent<Achievement, SOAchievement>
    {
        protected override Achievement Find(string name)
        {
            return values.Find((x) => x.name == name);
        }

        protected override List<Achievement> SetupData(SOAchievement[] data)
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

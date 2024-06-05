using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class AchievementManager : Singleton<AchievementManager>
    {
        public static Action<Achievement> onProgressed;
        public static Action<Achievement> onCompleted;

        public AssetArray<Achievement> achievements;

        public void Setup(Action<bool> callback) 
        {
            AssetManager.instance.GetAll((SOAchievement[] soAchievements) =>
            {
                List<Achievement> achievementList = new List<Achievement>();

                foreach(SOAchievement achievement in soAchievements)
                {
                    achievementList.Add(new Achievement(achievement));
                }

                achievements = new AssetArray<Achievement>(achievementList.ToArray());

                Dictionary<string, string> formFields = new Dictionary<string, string>()
                {
                    { "token", UserManager.instance.user.token }
                };

                Request.Post(ApplicationManager.instance.backend.achievementGet, formFields, (Request.Response response) =>
                {
                    if(response.success)
                    {
                        AchievementResponse data = JsonUtility.FromJson<AchievementResponse>(response.data);

                        foreach(AchievementResponse.Achievement achievement in data.achievements)
                        {
                            achievements.Get(achievement.name).value = achievement.value;
                        }

                        callback?.Invoke(true);
                    }
                    else
                    {
                        callback?.Invoke(false);
                    }
                });
            });
        }

        public void Set(string name, int value, Action<bool> callback = null)
        {
            Achievement achievement = achievements.Get(name);

            if(achievement == null)
            {
                callback?.Invoke(false);

                return;
            }

            if(value > achievement.maxValue)
            {
                callback?.Invoke(false);

                return;
            }

            if(value <= achievement.value)
            {
                callback?.Invoke(false);

                return;
            }

            achievement.value = value;

            onProgressed?.Invoke(achievement);

            if(achievement.completed)
            {
                onCompleted?.Invoke(achievement);
            }

            Dictionary<string, string> formFields = new Dictionary<string, string>()
            {
                { "token", UserManager.instance.user.token },
                { "name", name },
                { "value", value.ToString() }
            };

            Request.Post(ApplicationManager.instance.backend.achievementSet, formFields, (Request.Response response) =>
            {
                callback?.Invoke(response.success);
            });
        }
    }
}

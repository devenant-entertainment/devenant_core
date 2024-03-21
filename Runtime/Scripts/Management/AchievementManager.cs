using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class AchievementManager : Singleton<AchievementManager>
    {
        public static Action<Achievement> onProgressed;
        public static Action<Achievement> onCompleted;

        public Achievement[] achievements { get { return _achievements; } private set { _achievements = value; } }
        private Achievement[] _achievements;

        public void Setup(Action<bool> callback) 
        {
            DataManager.instance.achievementDataController.Get((Achievement[] achievements) =>
            {
                this.achievements = achievements;

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
                            Get(achievement.name).value = achievement.value;
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

        public Achievement Get(string name)
        {
            foreach(Achievement achievement in achievements)
            {
                if(name == achievement.name)
                {
                    return achievement;
                }
            }

            return null;
        }

        public void Set(string name, int value, Action<bool> callback = null)
        {
            Achievement achievement = Get(name);

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

            if(achievement.IsCompleted())
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

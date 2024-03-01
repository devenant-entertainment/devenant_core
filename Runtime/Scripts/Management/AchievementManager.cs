using Steamworks;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace Devenant
{
    public class AchievementManager : Singleton<AchievementManager>
    {
        public static Action<Achievement> onProgressed;
        public static Action<Achievement> onCompleted;

        public Achievement[] achievements { get { return _achievements; } private set { _achievements = value; } }
        private Achievement[] _achievements;

        public void Setup(AchievementData[] achievements, Action<bool> callback) 
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>()
            {
                { "token", UserManager.instance.user.token }
            };

            Request.Post(ApplicationManager.instance.backend.achievementGet, formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    AchievementResponse data = JsonUtility.FromJson<AchievementResponse>(response.data);

                    this.achievements = new Achievement[achievements.Length];

                    for(int i = 0; i < this.achievements.Length; i++)
                    {
                        int value = 0;

                        foreach(AchievementResponse.Achievement achievement in data.achievements)
                        {
                            if (achievement.name == achievements[i].name)
                            {
                                value = achievement.value;

                                break;
                            }
                        }

                        this.achievements[i] = new Achievement(achievements[i].name, achievements[i].icon, achievements[i].maxValue, value);
                    }

                    callback?.Invoke(true);
                }
                else
                {
                    callback?.Invoke(false);
                }
            });
        }

        public void Set(string name, int value, Action<bool> callback = null)
        {
            foreach(Achievement achievement in achievements)
            {
                if(achievement.name == name)
                {
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

                    if(achievement.value == achievement.maxValue)
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

                    return;
                }
            }

            callback?.Invoke(false);
        }
    }
}

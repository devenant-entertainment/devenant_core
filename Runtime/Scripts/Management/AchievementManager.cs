using UnityEngine;

namespace Devenant
{
    public class AchievementManager : Singleton<AchievementManager>
    {
        public static Action<Achievement> onProgressed;
        public static Action<Achievement> onCompleted;

        public Achievement[] achievements { get { return _achievements; } private set { _achievements = value; } }
        private Achievement[] _achievements;

        public void Setup(Achievement.Info[] achievements, Action<bool> callback) 
        {
            Request.Get(ApplicationManager.instance.config.endpoints.achievementGet, UserManager.instance.user.token, (Request.Response response) =>
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
                            if (achievement.id == achievements[i].id)
                            {
                                value = achievement.value;

                                break;
                            }
                        }

                        this.achievements[i] = new Achievement(achievements[i], value);
                        this.achievements[i].onProgressed += onProgressed;
                        this.achievements[i].onCompleted += onCompleted;
                    }

                    callback?.Invoke(true);
                }
                else
                {
                    callback?.Invoke(false);
                }
            });
        }
    }
}

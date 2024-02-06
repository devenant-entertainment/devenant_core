using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class AchievementManager : Singleton<AchievementManager>
    {
        public static Action<Achievement> onProgressed;
        public static Action<Achievement> onCompleted;

        public class Category
        {
            public readonly string id;
            public readonly Achievement[] achievements;

            public Category(string id, Achievement[] achievements)
            {
                this.id = id;
                this.achievements = achievements;
            }
        }

        public class Achievement
        {
            public class Info
            {
                public readonly string id;
                public readonly int max;

                public Info(string id)
                {
                    this.id = id;
                    max = 1;
                }

                public Info(string id, int max)
                {
                    this.id = id;
                    this.max = Mathf.Clamp(max, 1, int.MaxValue);
                }
            }

            public readonly Info info;

            public int value
            {
                get
                {
                    return _value;
                }
                set
                {
                    if(value > _value && value <= info.max)
                    {
                        _value = Mathf.Clamp(value, 0, info.max);

                        onProgressed?.Invoke(this);

                        if(_value == info.max)
                        {
                            onCompleted?.Invoke(this);
                        }
                    }
                }
            }
            private int _value;

            public bool completed { get { return value == info.max; } }

            public Achievement(Info info, int value, Action<Achievement> setup)
            {
                this.info = info;
                _value = value;

                setup?.Invoke(this);
            }

            public void Complete()
            {
                if(!completed)
                {
                    value = info.max;
                }
            }
        }

        private class AchievementData
        {
            public class Achievement
            {
                public string id;
                public int value;
            }

            public Achievement[] achievements;

            public int GetValue(string id)
            {
                foreach(Achievement achievement in achievements)
                {
                    if (achievement.id == id)
                    {
                        return achievement.value;
                    }
                }

                return 0;
            }
        }

        public Category[] categories { get { return _categories.ToArray(); } }
        private List<Category> _categories;

        public void Setup(Dictionary<string, Dictionary<Achievement.Info, Action<Achievement>>> data, Action<bool> callback)
        {
            Request.Get(Application.config.apiUrl + "achievement/get", UserManager.instance.data.token, (Request.Response response) =>
            {
                if(response.success)
                {
                    AchievementData achievementData = JsonUtility.FromJson<AchievementData>(response.data);

                    _categories = new List<Category>();

                    foreach(string category in data.Keys)
                    {
                        List<Achievement> achievements = new List<Achievement>();

                        foreach(Achievement.Info achievement in data[category].Keys)
                        {
                            achievements.Add(new Achievement(achievement, achievementData.GetValue(achievement.id), data[category][achievement]));
                        }

                        _categories.Add(new Category(category, achievements.ToArray()));
                    }
                }
            });
        }

        public void Set(string id, int value, Action<bool> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>()
            {
                { "achievement" ,  id},
                { "value" ,  value.ToString()},
            };

            Request.Post(Application.config.apiUrl + "achievement/get", formFields, UserManager.instance.data.token, (Request.Response response) =>
            {
                callback?.Invoke(response.success);
            });
        }
    }
}

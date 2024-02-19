using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class AchievementManager : Singleton<AchievementManager>
    {
        public static Action<Achievement> onProgressed;
        public static Action<Achievement> onCompleted;

        [System.Serializable]
        private class Response
        {
            public Achievement[] achievements;

            [System.Serializable]
            public class Achievement
            {
                public string id;
                public int value;
            }
        }

        public class Achievement
        {
            public class Info
            {
                public readonly string id;
                public readonly int value;

                public Info(string id, int value)
                {
                    this.id = id;
                    this.value = value;
                }
            }

            public readonly Info info;

            public int value { get { return _value; } private set { _value = value; } }
            private int _value;

            public Achievement(Info info, int value)
            {
                this.info = info;
                this.value = value;
            }

            public void Set(int value, Action<bool> callback = null)
            {
                if(value > info.value)
                {
                    callback?.Invoke(false);

                    return;
                }

                if(value <= this.value)
                {
                    callback?.Invoke(false);

                    return;
                }

                this.value = value;

                onProgressed?.Invoke(this);

                if(this.value == info.value)
                {
                    onCompleted?.Invoke(this);
                }

                Dictionary<string, string> formFields = new Dictionary<string, string>()
                {
                    {"id", info.id },
                    {"value", value.ToString() }
                };

                Request.Post(Application.config.gameApiUrl + "achievements/set", formFields, UserManager.instance.data.token, (Request.Response response) =>
                {
                    callback?.Invoke(response.success);
                });
            }
        }

        public Achievement[] achievements { get { return _achievements; } private set { _achievements = value; } }
        private Achievement[] _achievements;

        public void Setup(Achievement.Info[] achievements, Action<bool> callback) 
        {
            Request.Get(Application.config.gameApiUrl + "achievements/get", UserManager.instance.data.token, (Request.Response response) =>
            {
                if(response.success)
                {
                    Response data = JsonUtility.FromJson<Response>(response.data);

                    this.achievements = new Achievement[achievements.Length];

                    for(int i = 0; i < this.achievements.Length; i++)
                    {
                        int value = 0;

                        foreach(Response.Achievement achievement in data.achievements)
                        {
                            if (achievement.id == achievements[i].id)
                            {
                                value = achievement.value;

                                break;
                            }
                        }

                        this.achievements[i] = new Achievement(achievements[i], value);
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

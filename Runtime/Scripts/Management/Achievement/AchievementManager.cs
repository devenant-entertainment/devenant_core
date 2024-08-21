using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Devenant
{
    [RequireComponent(typeof(InitializableObject))]
    public class AchievementManager : Singleton<AchievementManager>, IInitializable
    {
        public static Action<AchievementData> onProgressed;
        public static Action<AchievementData> onCompleted;

        public EntityDataArray<AchievementData> achievements;

        public void Initialize(Action<InitializationResponse> callback)
        {
            Addressables.LoadAssetsAsync<AchievementAsset>(typeof(AchievementAsset).Name, null).Completed += (AsyncOperationHandle<IList<AchievementAsset>> asyncOperationHandle) =>
            {
                List<AchievementData> achievementList = new List<AchievementData>();

                foreach(AchievementAsset achievement in asyncOperationHandle.Result)
                {
                    achievementList.Add(new AchievementData(achievement));
                }

                achievements = new EntityDataArray<AchievementData>(achievementList.ToArray());

                Dictionary<string, string> formFields = new Dictionary<string, string>()
                {
                    { "token", UserManager.instance.data.token }
                };

                Request.Post(BackendManager.instance.data.achievementGet, formFields, (Request.Response response) =>
                {
                    if(response.success)
                    {
                        AchievementResponse data = JsonUtility.FromJson<AchievementResponse>(response.data);

                        foreach(AchievementResponse.Achievement achievement in data.achievements)
                        {
                            achievements.Get(achievement.name).value = achievement.value;
                        }

                        callback?.Invoke(new InitializationResponse(true));
                    }
                    else
                    {
                        callback?.Invoke(new InitializationResponse(false));
                    }
                });
            };
        }

        public void Set(string name, int value, Action<bool> callback = null)
        {
            AchievementData achievement = achievements.Get(name);

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
                { "token", UserManager.instance.data.token },
                { "name", name },
                { "value", value.ToString() }
            };

            Request.Post(BackendManager.instance.data.achievementSet, formFields, (Request.Response response) =>
            {
                callback?.Invoke(response.success);
            });
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Devenant
{
    [RequireComponent(typeof(InitializableObject))]
    public class AvatarManager : Singleton<AvatarManager>, IInitializable
    {
        public EntityDataArray<AvatarData> avatars;

        public void Initialize(Action<InitializationResponse> callback)
        {
            Addressables.LoadAssetsAsync<AvatarAsset>(typeof(AvatarAsset).Name, null).Completed += (AsyncOperationHandle<IList<AvatarAsset>> asyncOperationHandle) =>
            {
                List<AvatarData> avatarList = new List<AvatarData>();

                foreach(AvatarAsset avatar in asyncOperationHandle.Result)
                {
                    avatarList.Add(new AvatarData(avatar));
                }

                avatars = new EntityDataArray<AvatarData>(avatarList.ToArray());

                callback?.Invoke(new InitializationResponse(true));
            };
        }

        public AvatarData Get(string name)
        {
            foreach(AvatarData avatar in avatars.Get())
            {
                if (name == avatar.name)
                {
                    return avatar;
                }
            }

            foreach(AvatarData avatar in avatars.Get())
            {
                if(avatar.IsUnlocked())
                {
                    return avatar;
                }
            }

            return null;
        }
    }
}
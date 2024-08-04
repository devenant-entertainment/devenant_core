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
        public AssetArray<AvatarData> avatars;

        public void Initialize(Action<InitializationResponse> callback)
        {
            Addressables.LoadAssetsAsync<AvatarDataAsset>(typeof(AvatarDataAsset).ToString(), null).Completed += (AsyncOperationHandle<IList<AvatarDataAsset>> asyncOperationHandle) =>
            {
                List<AvatarData> avatarList = new List<AvatarData>();

                foreach(AvatarDataAsset avatar in asyncOperationHandle.Result)
                {
                    avatarList.Add(new AvatarData(avatar));
                }

                avatars = new AssetArray<AvatarData>(avatarList.ToArray());

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
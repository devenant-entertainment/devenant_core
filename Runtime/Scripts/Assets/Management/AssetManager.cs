using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Devenant
{
    public class AssetManager : Singleton<AssetManager>
    {
        private string GetAddressableGroup<T>() where T : SOAsset
        {
            return typeof(T).Name;
        }

        public void Get<T>(Action<T> callback) where T : SOAsset
        {
            Addressables.LoadAssetAsync<T>(GetAddressableGroup<T>()).Completed += (AsyncOperationHandle<T> asyncOperationHandle) =>
            {
                callback?.Invoke(asyncOperationHandle.Result);

                Addressables.Release(asyncOperationHandle);
            };
        }

        public void GetAll<T>(Action<T[]> callback) where T : SOAsset
        {
            Addressables.LoadAssetsAsync<T>(GetAddressableGroup<T>(), null).Completed += (AsyncOperationHandle<IList<T>> asyncOperationHandle)=>
            {
                callback?.Invoke(asyncOperationHandle.Result.ToArray());

                Addressables.Release(asyncOperationHandle);
            };
        }
    }
}

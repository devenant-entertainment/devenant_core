using System.Collections.Generic;

namespace Devenant
{
    public abstract class AssetDataController<T, D> where T : Asset where D : SOAsset
    {
        public void Get(Action<T[]> callback)
        {
            AssetManager.instance.Get((D[] result) =>
            {
                callback?.Invoke(NormalizeData(result).ToArray());
            });
        }

        public void Get(string name, Action<T> callback)
        {
            AssetManager.instance.Get((D[] result) =>
            {
                callback?.Invoke(NormalizeData(result).Find((x) => name == x.name));
            });
        }

        protected abstract List<T> NormalizeData(D[] data);
    }
}

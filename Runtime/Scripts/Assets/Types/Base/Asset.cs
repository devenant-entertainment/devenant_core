using UnityEngine;

namespace Devenant
{
    public class AssetArray<T> where T : Asset
    {
        private readonly T[] values;

        public AssetArray(T[] values)
        {
            this.values = values;
        }

        public T[] Get()
        {
            return values;
        }

        public T Get(string name)
        {
            foreach(T value in values)
            {
                if(value.name == name)
                {
                    return value;
                }
            }

            return null;
        }
    }

    public class Asset
    {
        public readonly string name;
        public readonly Sprite icon;

        public Asset(string name, Sprite icon)
        {
            this.name = name;
            this.icon = icon;
        }

        public Asset (SOAsset asset)
        {
            name = asset.name;
            icon = asset.icon;
        }
    }
}

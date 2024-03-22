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

        public Asset(string name)
        {
            this.name = name;
        }

        public Asset (SOAsset asset)
        {
            name = asset.name;
        }
    }
}

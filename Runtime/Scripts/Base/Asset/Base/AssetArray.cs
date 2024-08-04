namespace Devenant
{
    public class AssetArray<T> where T : AssetData
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
}

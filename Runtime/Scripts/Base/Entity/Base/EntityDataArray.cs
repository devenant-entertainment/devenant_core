namespace Devenant
{
    public class EntityDataArray<T> where T : EntityData
    {
        private readonly T[] values;

        public EntityDataArray(T[] values)
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

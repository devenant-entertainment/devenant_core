namespace Devenant
{
    public class Achievement
    {
        public readonly string id;
        public readonly int maxValue;

        public int value;

        public bool completed
        {
            get 
            {
                return value == maxValue; 
            }
        }

        public Achievement(string id, int maxValue, int value)
        {
            this.id = id;
            this.maxValue = maxValue;
            this.value = value;
        }
    }
}

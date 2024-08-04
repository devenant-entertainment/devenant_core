namespace Devenant
{
    [System.Serializable]
    public class AchievementDataResponse
    {
        public Achievement[] achievements;

        [System.Serializable]
        public class Achievement
        {
            public string name;
            public int value;
        }
    }
}

namespace Devenant
{
    [System.Serializable]
    public class AchievementResponse
    {
        public Achievement[] achievements;

        [System.Serializable]
        public class Achievement
        {
            public string id;
            public int value;
        }
    }
}

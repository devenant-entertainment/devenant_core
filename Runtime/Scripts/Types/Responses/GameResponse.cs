namespace Devenant
{
    [System.Serializable]
    public class GameResponse
    {
        public Game[] games;

        [System.Serializable]
        public class Game
        {
            public string id;
            public string name;
            public string date;
            public string data;
        }
    }
}

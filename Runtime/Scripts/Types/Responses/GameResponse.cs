namespace Devenant
{
    [System.Serializable]
    public class GameResponse
    {
        public Game[] games;

        [System.Serializable]
        public class Game
        {
            public string name;
        }
    }
}

namespace Devenant
{
    public class Game
    {
        public readonly string name;

        public Game(GameResponse.Game data)
        {
            name = data.name;
        }
    }
}

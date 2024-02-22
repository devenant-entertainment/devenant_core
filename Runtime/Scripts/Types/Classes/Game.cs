using System;

namespace Devenant
{
    public class Game
    {
        public readonly string id;
        public readonly string name;
        public readonly DateTime date;

        public Game(string id, string name, DateTime date)
        {
            this.id = id;
            this.name = name;
            this.date = date;
        }

        public Game(GameResponse.Game data)
        {
            id = data.id;
            name = data.name;
            date = DateTime.Parse(data.date);
        }
    }
}

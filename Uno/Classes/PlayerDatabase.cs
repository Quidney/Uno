using System.Collections.Generic;

namespace Uno.Classes
{
    public class PlayerDatabase
    {
        public List<Player> players;
        int lastID;

        public PlayerDatabase()
        {
            players = new List<Player>();
            lastID = 1;
        }

        public void AddPlayer(string name)
        {
            players.Add(new Player(lastID++, name));
        }
    }
}
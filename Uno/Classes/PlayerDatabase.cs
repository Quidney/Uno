using System.Collections.Generic;
using System.Windows.Forms;

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

        public void AddClientPlayer(string name)
        {
            Player clientPlayer = new Player(lastID++, name);
            clientPlayer.IsHost = false;
            players.Add(clientPlayer);

            MessageBox.Show($"Player {name} joined the game!");
        }

        public void AddHostPlayer(string name)
        {
            Player hostPlayer = new Player(lastID++, name);
            hostPlayer.IsHost = true;
            players.Add(hostPlayer);

        }
    }
}
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Uno.Classes
{
    public class PlayerDatabase
    {
        public List<Player> players;
        private CustomTextBox serverLog;
        public Dictionary<string, Player> playerDictionary;

        public PlayerDatabase()
        {
            players = new List<Player>();
            playerDictionary = new Dictionary<string, Player>();
        }

        public void SetReferences(CustomTextBox serverLog)
        {
            this.serverLog = serverLog;
        }

        public void AddClientPlayer(string name)
        {
            string playerID = GetUUIDFromUsername(name);   
            Player clientPlayer = new Player(playerID, name);
            clientPlayer.IsHost = false;
            players.Add(clientPlayer);
            playerDictionary.Add(playerID, clientPlayer);
            serverLog.AppendText($"Player {name} (UUID: {playerID}) joined the game!{Environment.NewLine}");
        }

        public void AddHostPlayer(string name)
        {
            string playerID = GetUUIDFromUsername(name);
            Player hostPlayer = new Player(playerID, name);
            hostPlayer.IsHost = true;
            players.Add(hostPlayer);
            playerDictionary.Add(playerID, hostPlayer);
            serverLog.AppendText($"Host {name} (UUID: {playerID}) created the server.{Environment.NewLine}");
        }
        public string GetUUIDFromUsername(string username)
        {
            // Namespace for UUID generation
            string namespaceUUID = "38400000-8cf0-11bd-b23e-10b96e4ef00d";

            // Concatenate namespace and username
            string combined = namespaceUUID + username.ToLower();

            // Calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(combined);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Set version (bits 4-7 of byte 7) and variant (bits 6-7 of byte 9) to appropriate values
                hashBytes[6] = (byte)((hashBytes[6] & 0x0F) | 0x30); // Set version to 0011 (version 3)
                hashBytes[8] = (byte)((hashBytes[8] & 0x3F) | 0x80); // Set variant to 10xx (variant 2)

                // Convert to UUID format
                Guid uuid = new Guid(hashBytes);

                return uuid.ToString();
            }
        }
    }
}
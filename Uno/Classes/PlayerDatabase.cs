﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Uno.Classes
{
    public class PlayerDatabase
    {
        public List<Player> players;
        private CustomRichTextBox serverLog;

        public Dictionary<string, string> UuidNameDictionary;
        public Dictionary<string, Player> UuidPlayerDictionary;
        public Dictionary<string, Player> NamePlayerDictionary;
        public Dictionary<Player, TcpClient> PlayerClientDictionary;

        Random random = new Random();

        public PlayerDatabase()
        {
            players = new List<Player>();
            UuidNameDictionary = new Dictionary<string, string>();
            UuidPlayerDictionary = new Dictionary<string, Player>();
            NamePlayerDictionary = new Dictionary<string, Player>();
            PlayerClientDictionary = new Dictionary<Player, TcpClient>();
        }

        public void SetReferences(CustomRichTextBox serverLog)
        {
            this.serverLog = serverLog;
        }

        public Player AddClientPlayer(string name)
        {
            string playerID = GetUUIDFromUsername(name);
            Player clientPlayer = new Player(playerID, name);
            clientPlayer.IsHost = false;

            players.Add(clientPlayer);
            NamePlayerDictionary.Add(clientPlayer.Name, clientPlayer);
            UuidNameDictionary.Add(playerID, clientPlayer.Name);
            UuidPlayerDictionary.Add(playerID, clientPlayer);

            return clientPlayer;
        }

        public Player AddHostPlayer(string name)
        {
            string playerID = GetUUIDFromUsername(name);
            Player hostPlayer = new Player(playerID, name);
            hostPlayer.IsHost = true;

            players.Add(hostPlayer);
            NamePlayerDictionary.Add(hostPlayer.Name, hostPlayer);
            UuidNameDictionary.Add(playerID, hostPlayer.Name);
            UuidPlayerDictionary.Add(playerID, hostPlayer);

            return hostPlayer;
        }

        public void RemovePlayer(Player player)
        {
            players.Remove(player);
            NamePlayerDictionary.Remove(player.Name);
            UuidNameDictionary.Remove(player.Id);
            UuidPlayerDictionary.Remove(player.Id);
        }

        public void ShufflePlayerOrder()
        {
            for (int i = 0; i < 5; i++)
            {
                int playerCount = players.Count;

                while (playerCount > 1)
                {
                    playerCount--;

                    int randomIndex = random.Next(playerCount + 1);

                    Player player = players[randomIndex];
                    players[randomIndex] = players[playerCount];
                    players[playerCount] = player;
                }
            }
        }

        public string GetUUIDFromUsername(string username)
        {
            string namespaceUUID = "38400000-8cf0-11bd-b23e-10b96e4ef00d";

            string combined = namespaceUUID + username.ToLower();

            // Calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(combined);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                hashBytes[6] = (byte)((hashBytes[6] & 0x0F) | 0x30); // Set version to 0011 (version 3)
                hashBytes[8] = (byte)((hashBytes[8] & 0x3F) | 0x80); // Set variant to 10xx (variant 2)

                // Convert to UUID format
                Guid uuid = new Guid(hashBytes);

                return uuid.ToString();
            }
        }


        public void ClearInventories()
        {
            foreach (Player player in players)
            {
                player.Inventory.Clear();
            }
        }

    }
}
﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Uno.Classes
{
    public class ServerHost
    {
        Player hostPlayer;
        TcpListener listener;
        List<TcpClient> clients;
        PlayerDatabase playerDatabase;

        Form1 form1;

        public ServerHost()
        {
            clients = new List<TcpClient>();
        }

        public void SetReferences(Form1 form1)
        {
            this.form1 = form1;
            this.playerDatabase = form1.playerDatabase;
        }

        public Player HostServer(int port, string username)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                hostPlayer = playerDatabase.AddHostPlayer(username);

                AcceptClients();

                return hostPlayer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                return null;
            }
        }

        private async void AcceptClients()
        {
            try
            {
                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();

                    clients.Add(client);

                    Thread newClientThread = new Thread(ClientConnection);
                    newClientThread.Start(client);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }

        }

        private async void ClientConnection(object clientInit)
        {
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                client = (TcpClient)clientInit;
                stream = client.GetStream();

                byte[] buffer = new byte[4096];
                int bytesRead;
                


                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    int messageType = buffer[0];
                    byte[] restOfData = new byte[bytesRead - 1];
                    Array.Copy(buffer, 1, restOfData, 0, bytesRead - 1);

                    string message = Encoding.ASCII.GetString(restOfData);
                    if (!ProcessMessage(message, client, messageType))
                    {
                        break;
                    }
                }
                stream?.Close();
                stream?.Dispose();
                clients.Remove(client);
                client?.Close();
                client?.Dispose();
            }
            catch (ObjectDisposedException)
            {

            }
            catch (IOException)
            {
                Player disconnectedPlayer = playerDatabase.players[clients.IndexOf(client) + 1];
                form1.AppendLogBox($"{disconnectedPlayer.Name} has disconnected.");

                client?.Close();
                clients.Remove(client);
                client?.Dispose();

                stream?.Close();
                stream?.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                MessageBox.Show(ex.ToString());
            }
        }

        private bool ProcessMessage(string message, TcpClient client, int messageType)
        {
            try
            {
                string command = message.Split(' ')[0].Trim();
                string senderString = message.Split(' ')[1].Trim();

                switch (command)
                {
                    case "MSG":
                        int skipSubstringMSG = command.Length + senderString.Length + 2;
                        string restOfMessageMSG = message.Substring(skipSubstringMSG);

                        playerDatabase.NamePlayerDictionary.TryGetValue(senderString, out Player senderPlayer);
                        int indexPlayer = playerDatabase.players.IndexOf(senderPlayer);
                        playerDatabase.indexColorDictionary.TryGetValue(indexPlayer, out Color playerColor);

                        form1.AppendChatBox(restOfMessageMSG, playerColor, senderPlayer.Name);
                        return true;
                    case "JOIN":
                        int skipSubstringJOIN = command.Length + senderString.Length + 1;
                        string restOfMessageJOIN = message.Substring(skipSubstringJOIN);


                        if (!playerDatabase.NamePlayerDictionary.TryGetValue(restOfMessageJOIN, out Player existingPlayer))
                        {
                            Player newPlayer = playerDatabase.AddClientPlayer(senderString);
                            form1.AppendLogBox($"Player {newPlayer.Name} has joined the server!");
                            BroadcastData($"JOIN {newPlayer.Name}");
                            return true;
                        }
                        else
                        {
                            SendDataToSpecificClient("ERR Failed to join the server. Reason: A user with the same name already exists.", client);
                            return false;
                        }
                    default:
                        MessageBox.Show("UNKNOWN MESSAGE");
                        return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "ProcessMessage");
                return false;
            }

        }

        public void BroadcastData(string message)
        {
            foreach (TcpClient client in clients)
            {
                NetworkStream stream = client.GetStream();

                byte[] buffer = Encoding.ASCII.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        public void SendDataToSpecificClient(string message, TcpClient client)
        {
            NetworkStream clientStream = client?.GetStream();

            byte[] buffer = Encoding.ASCII.GetBytes(message);
            clientStream?.Write(buffer, 0, buffer.Length);
        }

    }
}
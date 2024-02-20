using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
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
        Dictionary<TcpClient, Player> playerClientPair;

        PlayerDatabase playerDatabase;

        Form1 form1;
        ChatBox chatBox;

        public ServerHost()
        {
            clients = new List<TcpClient>();
            playerClientPair = new Dictionary<TcpClient, Player>();
        }

        public void SetReferences(Form1 form1)
        {
            this.form1 = form1;
            this.playerDatabase = form1.playerDatabase;
            this.chatBox = form1.chatBox;
        }

        public (Player,bool) HostServer(int port, string username)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                hostPlayer = playerDatabase.AddHostPlayer(username);

                AcceptClients();

                return (hostPlayer,true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                return (null,false);
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
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "AcceptClients");
            }

        }

        
        private async void ClientConnection(object clientInit)
        {
            
            TcpClient client = null;
            NetworkStream stream = null;
            bool sameNameDisconnection = false;
            try
            {
                client = (TcpClient)clientInit;
                stream = client.GetStream();

                byte[] buffer = new byte[4096];
                int bytesRead;


                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    (bool,bool) operationSuccess = ProcessMessage(message, client);
                    sameNameDisconnection = operationSuccess.Item2;
                    if (!operationSuccess.Item1)
                    {
                        break;
                    }
                }
            }
            catch (ObjectDisposedException)
            {

            }
            catch (IOException)
            {
                Player disconnectedPlayer = playerDatabase.players[clients.IndexOf(client) + 1];
                form1.AppendLogBox($"{disconnectedPlayer.Name} has disconnected.");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "ClientConnection");
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (!sameNameDisconnection)
                {
                    Player disconnectedPlayer = playerDatabase.players[clients.IndexOf(client) + 1];
                    playerDatabase.RemovePlayer(disconnectedPlayer);
                }

                stream?.Close();
                stream?.Dispose();
                clients.Remove(client);
                client?.Close();
                client?.Dispose();
            }
        }

        private (bool,bool) ProcessMessage(string message, TcpClient client)
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

                        chatBox.AppendChatBox(restOfMessageMSG, Color.Blue, senderPlayer.Name);
                        return (true, false);
                    case "JOIN":
                        int skipSubstringJOIN = command.Length + senderString.Length + 1;
                        string restOfMessageJOIN = message.Substring(skipSubstringJOIN);


                        if (!playerDatabase.NamePlayerDictionary.TryGetValue(restOfMessageJOIN, out Player existingPlayer))
                        {
                            if ((playerDatabase.players.Count <= 3))
                            {
                                Player newPlayer = playerDatabase.AddClientPlayer(senderString);
                                playerClientPair.Add(client, newPlayer);
                                form1.AppendLogBox($"{newPlayer.Name} has joined the server!");
                                BroadcastData($"JOIN {newPlayer.Name}");

                                form1.AddPlayerToGUI(playerDatabase.players.Count - 1, newPlayer);

                                return (true,false);
                            }
                            else
                            {
                                SendDataToSpecificClient("ERR Server is full. Disconnecting...", client);
                                return (false, false);
                            }
                        }
                        else
                        {
                            SendDataToSpecificClient("ERR Failed to join the server. Reason: A user with the same name already exists.", client);
                            return (false,true);
                        }
                    default:
                        MessageBox.Show("UNKNOWN MESSAGE");
                        return (true, false);
                }
            }
            catch (ArgumentException argEx)
            {
                form1.AppendLogBox($"A user tried to connect with an already existing name. ({argEx.Message})");
                SendDataToSpecificClient("ERR A user with the same name aleady exists", client);
                return (false, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "ProcessMessage - Server");
                return (false, false);
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uno.Class;

namespace Uno.Classes
{
    public class ServerHost
    {
        Player hostPlayer;
        TcpListener listener;

        public List<TcpClient> clients;
        Dictionary<TcpClient, Player> playerClientPair;

        CardFunctionality cardFunctionality;
        PlayerDatabase playerDatabase;
        Deck deck;

        frmUno form1;
        ChatBox chatBox;

        public ServerHost()
        {
            clients = new List<TcpClient>();
            playerClientPair = new Dictionary<TcpClient, Player>();
        }

        public void SetReferences(frmUno form1)
        {
            this.form1 = form1;
            this.playerDatabase = form1.playerDatabase;
            this.chatBox = form1.chatBox;
            this.deck = form1.deck;
            this.cardFunctionality = form1.cardFunctionality;
        }

        public (Player, bool) HostServer(int port, string username)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                hostPlayer = playerDatabase.AddHostPlayer(username);

                AcceptClients();

                return (hostPlayer, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Host Server");
                return (null, false);
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
            catch (ObjectDisposedException)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "AcceptClients");
                throw;
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
                    (bool, bool) operationSuccess = await ProcessMessage(message, client);
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
                MessageBox.Show(ex.ToString(), "ClientConnection - Exception");
            }
            finally
            {
                try
                {
                    if (!form1.shuttingDown)
                    {
                        if (!sameNameDisconnection)
                        {
                            Player disconnectedPlayer = playerDatabase.players[clients.IndexOf(client) + 1];
                            form1.RemovePlayerFromGUI(playerDatabase.players.IndexOf(disconnectedPlayer));
                            playerDatabase.RemovePlayer(disconnectedPlayer);

                            if (playerDatabase.players.Count < 2)
                            {
                                form1.StartGameButtonState(false);
                            }
                        }
                    }

                    stream?.Close();
                    stream?.Dispose();
                    clients.Remove(client);
                    client?.Close();
                    client?.Dispose();

                    if (form1.isStarted)
                    {
                        form1.DisconnectedFromServerHost();

                        listener.Stop();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "ClientConnection - Finally");
                }

            }
        }

        private async Task<(bool, bool)> ProcessMessage(string message, TcpClient client)
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
                                playerDatabase.PlayerClientDictionary.Add(newPlayer, client);
                                playerClientPair.Add(client, newPlayer);
                                form1.AppendLogBox($"{newPlayer.Name} has joined the server!");
                                SendDataToSpecificClient("WELCOME", client);
                                await SendDataToAllExcept($"JOIN {newPlayer.Name}", client);

                                form1.AddPlayerToGUI(playerDatabase.players.Count - 1, newPlayer);

                                if (playerDatabase.players.Count > 1 && playerDatabase.players.Count < 5)
                                {
                                    form1.StartGameButtonState(true);
                                }

                                return (true, false);
                            }
                            else
                            {
                                SendDataToSpecificClient("ERR Server is full. Disconnecting...", client);
                                return (false, true);
                            }
                        }
                        else
                        {
                            SendDataToSpecificClient("ERR Failed to join the server. Reason: A user with the same name already exists.", client);
                            return (false, true);
                        }

                    case "PLAY":
                        int playedCardID = Convert.ToInt32(message.Split(' ')[2].Trim());
                        if (deck.idToCard.TryGetValue(playedCardID, out Card playedCard))
                        {
                            if (playerDatabase.NamePlayerDictionary.TryGetValue(senderString, out Player playingPlayer))
                            {
                                if (playingPlayer.Inventory.Contains(playedCard))
                                {
                                    if (cardFunctionality.ThrowCardInPile(playedCard, playingPlayer))
                                    {
                                       await SendDataToAllExcept($"PLAY {playingPlayer.Name} {playedCardID}", client);
                                        if (form1.InvokeRequired)
                                            form1.Invoke(new Action(form1.SetInventoryGUI));
                                        else
                                            form1.SetInventoryGUI();
                                    }
                                    else
                                    {
                                        SendDataToSpecificClient("ERR HUH?!!?!?", client);
                                    }
                                }
                                else
                                {
                                    SendDataToSpecificClient("ERR You don't have that card in your inventory!!?", client);
                                    MessageBox.Show("Player played a card that's not in it's inventory.", "ProcessMessage - Case PLAY");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Cannot parse player", "ProcessMessage - Case PLAY");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Wrong CARD ID", "ProcessMessage - Case PLAY");
                        }

                        return (true, false);
                    case "CHANGECOLOR":
                        Enum.TryParse<Card.ColorEnum>(message.Split(' ')[1], out Card.ColorEnum colorToChange);
                        cardFunctionality.currentColor = colorToChange;
                        await SendDataToAllExcept(message, client);
                        return (true, false);
                    default:
                        form1.AppendLogBox("Unknown Message Received: " + message + " by: " + client);
                        return (true, false);
                }
            }
            catch (ArgumentException argEx)
            {
                if (argEx.Message.Contains ("same key"))
                {
                    form1.AppendLogBox($"A user tried to connect with an already existing name. ({argEx.Message})");
                    SendDataToSpecificClient("ERR A user with the same name aleady exists", client);
                    return (false, true);
                }
                else
                {
                    throw;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "ProcessMessage - Server");
                return (false, false);
            }

        }

        public async Task BroadcastData(string message)
        {
            foreach (TcpClient client in clients)
            {
                NetworkStream stream = client.GetStream();

                string delimiter = "\n";

                string messageWithDelimiter = message + delimiter;

                byte[] buffer = Encoding.ASCII.GetBytes(messageWithDelimiter);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public async void SendDataToSpecificClient(string message, TcpClient client)
        {
            NetworkStream clientStream = client?.GetStream();

            string delimiter = "\n";

            string messageWithDelimiter = message + delimiter;

            byte[] buffer = Encoding.ASCII.GetBytes(messageWithDelimiter);
            await clientStream?.WriteAsync(buffer, 0, buffer.Length);
        }

        public async Task SendDataToAllExcept(string message, TcpClient clientInit)
        {
            foreach (TcpClient client in clients)
            {
                if (client != clientInit)
                {
                    NetworkStream stream = client.GetStream();

                    string delimiter = "\n";

                    string messageWithDelimiter = message + delimiter;

                    byte[] buffer = Encoding.ASCII.GetBytes(messageWithDelimiter);
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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

        public string scorePath = @"./scores.txt";
        public int score = 0;

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

        //Host server. Hosting the server returns "Player + true",
        //failing to host (for example because the port is already used by another process) returns "null + false"
        public (Player, bool) HostServer(int port, string username)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                hostPlayer = playerDatabase.AddHostPlayer(username);

                List<string> lines = File.ReadAllLines(scorePath).ToList();

                bool usernameExists = false;
                foreach (string line in lines)
                {
                    string[] splittedLine = line.Split(';');
                    if (splittedLine[0].Equals(username))
                    {
                        score = int.Parse(splittedLine[1]);
                        usernameExists = true;
                    }
                }

                if (!usernameExists)
                {
                    lines.Add(username + ";0");
                    File.WriteAllLines(scorePath, lines);
                }


                AcceptClients();

                return (hostPlayer, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Host Server");
                return (null, false);
            }
        }

        //Accept clients async. When a client connects, create a new Thread for it, then start awaiting another client.
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

        //This method is created as a new Thread. So that multiple clients can connect.
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
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
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
                //Process message (string) and do something based on the message

                string command = message.Split(' ')[0].Trim();
                string senderString = message.Split(' ')[1].Trim();


                //Switch (-string command-)
                switch (command)
                {
                    case "MSG":
                        //Command Message. Syntax "MESSAGE USERNAME MESSAGE_CONTENT"
                        //ChatBox.
                        int skipSubstringMSG = command.Length + senderString.Length + 2;
                        string restOfMessageMSG = message.Substring(skipSubstringMSG);

                        playerDatabase.NamePlayerDictionary.TryGetValue(senderString, out Player senderPlayer);
                        int indexPlayer = playerDatabase.players.IndexOf(senderPlayer);

                        if (chatBox.InvokeRequired)
                            chatBox.Invoke(new Action(() => { chatBox.AppendChatBox(restOfMessageMSG, Color.Blue, senderPlayer.Name); }));
                        else
                        chatBox.AppendChatBox(restOfMessageMSG, Color.Blue, senderPlayer.Name);

                        await SendDataToAllExcept($"MSG {senderString} {restOfMessageMSG}", client);

                        if (restOfMessageMSG.ToLower().Contains("uno"))
                        {
                            if (!senderPlayer.turn)
                            {
                                if (senderPlayer.Inventory.Count == 1)
                                {
                                    senderPlayer.SaidUno = true;
                                    if (chatBox.InvokeRequired)
                                        chatBox.Invoke(new Action(() => { chatBox.AppendChatBox(senderPlayer.Name + " said Uno!", Color.Red, "Server"); }));
                                    else
                                        chatBox.AppendChatBox(senderPlayer.Name + " said Uno!", Color.Red, "Server");

                                    SendDataToSpecificClient("MSG Server You said uno", client);
                                    await SendDataToAllExcept($"MSG Server {senderPlayer.Name} said Uno!", client);
                                }
                                else
                                {
                                    if (chatBox.InvokeRequired)
                                        chatBox.Invoke(new Action(() => { chatBox.AppendChatBox($"Player {senderPlayer.Name} said UNO, but he/she doesn't have 1 card in their inventory.", Color.Red, "Server"); }));
                                    else
                                        chatBox.AppendChatBox($"Player {senderPlayer.Name} said UNO, but he/she doesn't have 1 card in their inventory.", Color.Red, "Server");
                                }
                            }
                        }
                        return (true, false);
                    case "JOIN":
                        //Command Join. Syntax: "JOIN USERNAME SCORE"
                        //This command is sent to the server by the joining Client.
                        int skipSubstringJOIN = command.Length + senderString.Length + 1;
                        string score = message.Split(' ')[2];
                        string restOfMessageJOIN = message.Split(' ')[1];
                        

                        if (!playerDatabase.NamePlayerDictionary.TryGetValue(restOfMessageJOIN, out Player existingPlayer))
                        {
                            if ((playerDatabase.players.Count <= 3))
                            {
                                Player newPlayer = playerDatabase.AddClientPlayer(senderString);
                                playerDatabase.PlayerClientDictionary.Add(newPlayer, client);
                                playerClientPair.Add(client, newPlayer);

                                if (form1.InvokeRequired)
                                    form1.Invoke(new Action(() => { form1.AppendLogBox($"{newPlayer.Name} has joined the server with {score} amount of wins!"); }));
                                else
                                form1.AppendLogBox($"{newPlayer.Name} has joined the server!");

                                SendDataToSpecificClient("WELCOME", client);
                                await SendDataToAllExcept($"JOIN {newPlayer.Name}", client);

                                if (form1.InvokeRequired)
                                    form1.Invoke(new Action(() => { form1.AddPlayerToGUI(playerDatabase.players.Count - 1, newPlayer); }));
                                else
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

                        //Command play. Syntax: "PLAY USERNAME CARD_ID"
                        //This is sent to the server when a player is trying to play a card.
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
                        //Command ChangeColor. Syntax: "CHANGECOLOR COLOR_ENUM"
                        //This is sent when a client changes the color.
                        Enum.TryParse<Card.ColorEnum>(message.Split(' ')[1], out Card.ColorEnum colorToChange);
                        cardFunctionality.currentColor = colorToChange;
                        await SendDataToAllExcept(message, client);
                        return (true, false);
                    case "DECK":
                        //Command Deck. Syntax: "DECK USERNAME"
                        //This command is sent by the client when it draws a card from the deck.
                        string playerNameDeck = message.Split(' ')[1];
                        playerDatabase.NamePlayerDictionary.TryGetValue(playerNameDeck, out Player player);
                        cardFunctionality.DrawCardsFromDeck(player, 1);
                        return (true, false);
                    default:
                        form1.AppendLogBox("Unknown Message Received: " + message + " by: " + client);
                        return (true, false);
                }
            }
            catch (ArgumentException argEx)
            {
                if (argEx.Message.Contains("same key"))
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

        //Send data to all the clients in the list.
        public async Task BroadcastData(string message)
        {
            foreach (TcpClient client in clients)
            {
                NetworkStream stream = client.GetStream();

                string delimiter = "\n";

                string messageWithDelimiter = message + delimiter;

                byte[] buffer = Encoding.UTF8.GetBytes(messageWithDelimiter);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        //Send data to only one client.
        public async void SendDataToSpecificClient(string message, TcpClient client)
        {
            try
            {
                NetworkStream clientStream = client?.GetStream();

                string delimiter = "\n";

                string messageWithDelimiter = message + delimiter;

                byte[] buffer = Encoding.UTF8.GetBytes(messageWithDelimiter);
                await clientStream?.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "SendDataToSpecificClient - ServerHost");
            }
        }

        //Send data to all the clients except specified. (For example when a client joins the game)
        public async Task SendDataToAllExcept(string message, TcpClient clientInit)
        {
            foreach (TcpClient client in clients)
            {
                if (client != clientInit)
                {
                    NetworkStream stream = client.GetStream();

                    string delimiter = "\n";

                    string messageWithDelimiter = message + delimiter;

                    byte[] buffer = Encoding.UTF8.GetBytes(messageWithDelimiter);
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
        }

    }
}
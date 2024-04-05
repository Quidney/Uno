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
    public class ServerJoin
    {
        Player player;
        TcpClient client;
        NetworkStream stream;

        frmUno form1;
        ChatBox chatBox;
        PlayerDatabase playerDatabase;
        CardFunctionality cardFunctionality;
        Deck deck;

        public string scorePath = @"./scores.txt";
        public int score = 0;

        public ServerJoin()
        {

        }

        public void SetReferences(frmUno form1)
        {
            this.form1 = form1;
            this.playerDatabase = form1.playerDatabase;
            this.chatBox = form1.chatBox;
            this.deck = form1.deck;
            this.cardFunctionality = form1.cardFunctionality;
        }


        // Try to connect to a server, return "Player + true" if it works, return "null + false" if it fails.
        public async Task<(Player, bool)> JoinGame(IPAddress ip, int port, string username)
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(ip, port);
                stream = client.GetStream();

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

                Thread joinServerThread = new Thread(ServerConnection);
                joinServerThread.Start();

                await SendDataToServer($"JOIN {username} {score}");

                player = playerDatabase.AddClientPlayer(username);

                return (player, true);
            }
            catch (SocketException sEx)
            {
                switch (sEx.SocketErrorCode)
                {
                    case SocketError.ConnectionRefused:
                        form1.AppendLogBox("No connection could be made because the target machine actively refused it.");
                        form1.AppendLogBox("Host is up but is refusing connections. Try double-checking the port.");
                        break;
                    case SocketError.TimedOut:
                        form1.AppendLogBox("Connection Timed Out");
                        break;
                }
                return (null, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "JoinGame ex");
                return (null, false);
            }
        }

        private StringBuilder messageBuffer = new StringBuilder();
        private async void ServerConnection()
        {
            try
            {
                byte[] buffer = new byte[4096];
                int bytesRead;

                //While the received data is more than 0 (the client isn't disconnected), await another message
                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    //Decode the message using UTF8
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    //Send the decoded string to a method to be processed.
                    ProcessReceivedData(receivedData);
                }
            }
            catch (ObjectDisposedException) { }
            catch (ArgumentException argEx)
            {
                form1.AppendLogBox($"You have disconnected. (Reason: {argEx.Message})");
            }
            catch (IOException ioEx)
            {
                form1.AppendLogBox($"You have disconnected. (Reason: {ioEx.Message})");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "ServerConnection - ServerJoin - Exception");
            }
            finally
            {
                stream?.Close();
                stream?.Dispose();
                client?.Close();
                client?.Dispose();
                form1.AppendLogBox("Disconnected from the server.");
                form1.DisconnectedFromServerClient();
            }
        }

        private void ProcessReceivedData(string receivedData)
        {
            //Split messages by delimiter. This is to ensure if multiple messages are written to the stream at the same time
            //(in a loop for example) then the messages still work as intended.
            string delimiter = "\n";

            messageBuffer.Append(receivedData);

            string[] messages = messageBuffer.ToString().Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string message in messages)
            {
                ProcessMessage(message);
            }

            int lastDelimiterIndex = messageBuffer.ToString().LastIndexOf(delimiter);
            if (lastDelimiterIndex >= 0)
            {
                messageBuffer.Remove(0, lastDelimiterIndex + delimiter.Length);
            }
        }

        private void ProcessMessage(string message)
        {
            try
            {
                //Process the message received from the server.

                string command = message.Split(' ')[0].Trim();
                //MessageBox.Show($"\"{message}\"", "ProcessMessage - ServerJoin");
                switch (command)
                {
                    case "MSG":
                        //Command Message. Syntax: "MSG PLAYERNAME MESSAGE"
                        //This command is sent when using the ChatBox
                        string senderString = message.Split(' ')[1].Trim();
                        int skipSubstringMSG = command.Length + senderString.Length + 2;
                        string restOfMessageMSG = message.Substring(skipSubstringMSG);
                        if (chatBox.InvokeRequired)
                            chatBox.Invoke(new Action(() => { chatBox.AppendChatBox(restOfMessageMSG, Color.Blue, senderString); }));
                        else
                            chatBox.AppendChatBox(restOfMessageMSG, Color.Blue, senderString);
                        break;

                    case "JOIN":
                        //Command Join. Syntax: "JOIN USERNAME"
                        //This is sent to the CLIENT when another player joins the server.
                        int skipSubstringJOIN = command.Length + 1;
                        string restOfMessageJOIN = message.Substring(skipSubstringJOIN);

                        if (form1.InvokeRequired)
                            form1.Invoke(new Action(() => { form1.AppendLogBox(restOfMessageJOIN + " has joined the server with " + score + " amount of wins!"); }));
                        else
                            form1.AppendLogBox(restOfMessageJOIN + " has joined the server with" + score + " amount of wins!");
                        break;

                    case "ERR":
                        //Command Error. Syntax: "ERR ERROR_MESSAGE"
                        //Sent when there's an exception in the server regarding the Client.
                        //For example; A server with the same name joins, then the newly joined player is kicked with the message
                        //"A player with the same name already joined".
                        int skipSubstringERR = command.Length + 1;
                        string restOfMessageERR = message.Substring(skipSubstringERR);
                        if (form1.InvokeRequired)
                            form1.Invoke(new Action(() => { form1.AppendLogBox(restOfMessageERR); }));
                        else
                            form1.AppendLogBox(restOfMessageERR);

                        break;

                    case "WELCOME":
                        //Comamnd Welcome. Syntax: "Welcome".
                        //This is sent once to the client after join. To setup certain aspects.
                        if (form1.InvokeRequired)
                            form1.Invoke(new Action(() => { form1.AppendLogBox("Connected to server"); }));
                        else
                            form1.AppendLogBox("Connected to server");

                        break;

                    case "DRAW":
                        //Command Draw. Syntax: "DRAW CARD_ID"
                        //This is regulated by the server, and sent to the Client which cards it should draw.
                        string cardIdStr = message.Split(' ')[1];
                        int cardID = Convert.ToInt32(cardIdStr);
                        deck.idToCard.TryGetValue(cardID, out Card cardToDraw);
                        player.AddCardToInventory(cardToDraw);
                        break;
                    case "START":
                        //Command Start. Syntax: "START"
                        //This is sent when the game is started.
                        if (form1.InvokeRequired)
                        {
                            form1.Invoke(new Action(form1.StartGameClient));
                        }
                        else
                            form1.StartGameClient();
                        break;
                    case "PLAY":
                        //Command Play. Syntax: "PLAY USERNAME CARD_ID"
                        //This is sent when a new card is played (so the Client can update their GUI).
                        deck.idToCard.TryGetValue(Convert.ToInt32(message.Split(' ')[2]), out Card card);
                        cardFunctionality.ThrowCardInPileForClient(card);
                        if (form1.InvokeRequired)
                            form1.Invoke(new Action(form1.SetInventoryGUI));
                        else
                            form1.SetInventoryGUI();
                        break;
                    case "PILE":
                        //Command Pile. Syntax: "PILE CARD_ID"
                        //Set new card on top of the pile.
                        deck.idToCard.TryGetValue(Convert.ToInt32(message.Split(' ')[1]), out Card cardOnPile);
                        form1.lastCardPlayed = cardOnPile;
                        cardFunctionality.currentColor = form1.lastCardPlayed.Color;
                        if (form1.InvokeRequired)
                            form1.Invoke(new Action(form1.SetInventoryGUI));
                        else
                            form1.SetInventoryGUI();
                        break;
                    case "CHANGECOLOR":
                        //Command ChangeColor. Syntax: "CHANGECOLOR COLOR_ENUM"
                        //Changes the color of the game. This is for when "Change Color Wild card" is played.
                        Enum.TryParse<Card.ColorEnum>(message.Split(' ')[1], out Card.ColorEnum colorToChange);
                        cardFunctionality.currentColor = colorToChange;
                        if (form1.InvokeRequired)
                            form1.Invoke((Action)(() => cardFunctionality.currentColorLabel.Text = colorToChange.ToString()));
                        else
                            cardFunctionality.currentColorLabel.Text = colorToChange.ToString();

                        break;
                    case "KICK":
                        //Command Kick. Syntax "KICK"
                        //Kicks the client from the server.
                        //Can be done via console or happen on same-name-conflict
                        if (form1.InvokeRequired)
                            form1.Invoke(new Action(() => { form1.AppendLogBox("Kicked from the server."); }));
                        else
                            form1.AppendLogBox("Kicked from the server.");
                        stream?.Close();
                        stream?.Dispose();
                        client?.Close();
                        client?.Dispose();
                        if (form1.InvokeRequired)
                            form1.Invoke(new Action(() =>
                            {
                                form1.DisconnectedFromServerClient();
                            }));
                        else
                            form1.DisconnectedFromServerClient();

                        break;
                    case "TURN":
                        //Command Turn. Syntax: "TURN"
                        //Gives the turn to the client.

                        cardFunctionality.canPlay = true;
                        if (form1.InvokeRequired)
                            form1.Invoke(new Action(() => { form1.Text = "Uno! Your Turn!"; }));
                        else
                            form1.Text = "Uno! Your Turn!";
                        break;
                    case "TURNOTHER":
                        //Command TurnOther. Syntax: "TURNOTHER USERNAME".
                        //Tells the client that another player has the turn.
                        string username = message.Split(' ')[1];
                        string cardsLeft = "7";
                        if (message.Split(' ').Length == 3)
                            cardsLeft = message.Split(' ')[2];

                        if (form1.InvokeRequired)
                            form1.Invoke(new Action(() => { form1.Text = $"Uno! {username}'s Turn! ({cardsLeft} Cards left)"; }));
                        else
                            form1.Text = $"Uno! {username}'s Turn! ({cardsLeft} Cards left)";
                        break;
                    case "CHEATS":
                        //Command Cheats. Syntax "CHEATS"
                        //This is sent to all Clients when the Server admin uses commands to check a players cards.
                        chatBox.AppendChatBox("THE ADMIN USED COMMANDS TO LOOK AT A PLAYERS CARDS!", Color.Red, "SERVER");
                        break;
                    default:
                        MessageBox.Show(message + "\nPlease tell the developer what you were doing when this occured.", "Unknown Message");
                        break;

                    case "WIN":
                        //Command Win. Syntax "WIN USERNAME"
                        //This command is sent when someone wins the game.
                        if (form1.InvokeRequired)
                            form1.Invoke(new Action(() => { form1.AppendLogBox($"{message.Split(' ')[1]} Won the game!"); }));
                        else
                            form1.AppendLogBox($"{message.Split(' ')[1]} Won the game!");

                        if (form1.InvokeRequired)
                            form1.Invoke(new Action(() =>
                            {
                                form1.GameWon(message.Split(' ')[1]);
                            }));
                        else
                            form1.GameWon(message.Split(' ')[1]);
                        break;
                    case "CLEARINV":
                        //Command ClearInventory. Syntax: "CLEARINV".
                        //This is sent to a client when the game is reset and the inventory must be cleared.
                        playerDatabase.ClearInventories();
                        cardFunctionality.canPlay = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "ProcessMessage - Client");
            }
        }

        //Send data to server by writing to the stream
        public async Task SendDataToServer(string message)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (IOException)
            {
                chatBox.AppendChatBox("Your Message was not sent because you are not connected to a server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "SendDataToServer - ServerJoin");
            }
        }
    }
}

using System;
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
    public class ServerJoin
    {
        Player player;
        TcpClient client;
        NetworkStream stream;

        Form1 form1;
        ChatBox chatBox;
        PlayerDatabase playerDatabase;
        Deck deck;
        public ServerJoin()
        {

        }

        public void SetReferences(Form1 form1)
        {
            this.form1 = form1;
            this.playerDatabase = form1.playerDatabase;
            this.chatBox = form1.chatBox;
            this.deck = form1.deck;
        }

        public async Task<(Player, bool)> JoinGame(IPAddress ip, int port, string username)
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(ip, port);
                stream = client.GetStream();

                Thread joinServerThread = new Thread(ServerConnection);
                joinServerThread.Start();

                SendDataToServer($"JOIN {username}");

                player = playerDatabase.AddClientPlayer(username);

                return (player, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
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

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    ProcessReceivedData(receivedData);
                }
            }
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
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                stream?.Close();
                stream?.Dispose();
                client?.Close();
                client?.Dispose();
                form1.AppendLogBox("Disconnected from the server.");
                form1.DisconnectedFromServer();
            }
        }

        private void ProcessReceivedData(string receivedData)
        {
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
                string command = message.Split(' ')[0].Trim();
                //MessageBox.Show($"\"{message}\"");
                switch (command)
                {
                    case "MSG":
                        string senderString = message.Split(' ')[1].Trim();
                        int skipSubstringMSG = command.Length + senderString.Length + 2;
                        string restOfMessageMSG = message.Substring(skipSubstringMSG);
                        chatBox.AppendChatBox(restOfMessageMSG, Color.Blue, senderString);
                        break;

                    case "JOIN":
                        int skipSubstringJOIN = command.Length + 1;
                        string restOfMessageJOIN = message.Substring(skipSubstringJOIN);
                        form1.AppendLogBox(restOfMessageJOIN + " has joined the server!");
                        break;

                    case "ERR":
                        int skipSubstringERR = command.Length + 1;
                        string restOfMessageERR = message.Substring(skipSubstringERR);
                        form1.AppendLogBox(restOfMessageERR);
                        break;

                    case "WELCOME":
                        form1.AppendLogBox("Connected to server");
                        break;

                    case "DRAW":
                        string cardIdStr = message.Split(' ')[1]; 
                        int cardID = Convert.ToInt32(cardIdStr);
                        deck.idToCard.TryGetValue(cardID, out Card cardToDraw);
                        player.AddCardToInventory(cardToDraw);
                        break;
                    case "START":
                        form1.StartGameClient();
                        break;
                    default:
                        MessageBox.Show(message + "\nPlease tell the developer what you were doing when this occured.", "Unknown Message");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "ProcessMessage - Client");
            }

        }

        public void SendDataToServer(string message)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (IOException)
            {
                chatBox.AppendChatBox("Your Message was not sent because you are not connected to a server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}

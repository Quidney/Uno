using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public ServerJoin()
        {

        }

        public void SetReferences(Form1 form1)
        {
            this.form1 = form1;
            this.playerDatabase = form1.playerDatabase;
            this.chatBox = form1.chatBox;

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

        private async void ServerConnection()
        {
            try
            {
                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    ProcessMessage(message);
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

        private void ProcessMessage(string message)
        {
            try
            {
                string command = message.Split(' ')[0].Trim();

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
                    default:
                        MessageBox.Show("UNKNOWN MESSAGE");
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

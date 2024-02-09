using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uno.Classes
{
    public class ClientHandler
    {
        private TcpClient client;
        private NetworkStream stream;

        private PlayerDatabase playerDatabase;
        private CardFunctionality cardFunctionality;

        private CustomRichTextBox chatBox;
        private CustomTextBox serverLog;

        private string clientUsername;
        private string clientUUID;
        private Player clientPlayer;

        public event EventHandler ClientDisconnected;

        public ClientHandler(TcpClient client)
        {
            this.client = client;
        }

        public void SetReferences(PlayerDatabase playerDatabase, CardFunctionality cardFunctionality, CustomTextBox serverLog, CustomRichTextBox chatBox)
        {
            this.playerDatabase = playerDatabase;
            this.cardFunctionality = cardFunctionality;
            this.serverLog = serverLog;
            this.chatBox = chatBox;
        }

        public async Task Start()
        {
            try
            {
                stream = client.GetStream();
                await HandleClient();
                OnClientDisconnected();
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        private void OnClientDisconnected()
        {
            ClientDisconnected?.Invoke(this, EventArgs.Empty);
        }


        private async Task HandleClient()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    await ProcessClientData(dataReceived);
                }
            }
            catch (Exception ex)
            {
                //UpdateServerLog($"Error in HandleClient: {ex.Message}{Environment.NewLine}");
                MessageBox.Show($"Error in HandleClient: {ex.StackTrace}{Environment.NewLine}");
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }

                if (clientPlayer != null)
                {
                    playerDatabase.players.Remove(clientPlayer);

                    if (!string.IsNullOrEmpty(clientUUID))
                    {
                        playerDatabase.UuidPlayerDictionary.Remove(clientUUID);
                        playerDatabase.UuidNameDictionary.Remove(clientUUID);
                    }
                }

                UpdateServerLog($"{clientUsername ?? "Unknown user"} disconnected.{Environment.NewLine}");
            }
        }
        private async Task ProcessClientData(string dataReceived)
        {
            if (dataReceived.StartsWith("JOIN"))
            {
                string playerName = dataReceived.Substring(5);
                playerDatabase.AddClientPlayer(playerName);

                clientUsername = playerName;
                clientUUID = playerDatabase.UuidNameDictionary.FirstOrDefault(x => x.Value == clientUsername).Key;
                playerDatabase.UuidPlayerDictionary.TryGetValue(clientUUID, out clientPlayer);
            }
            else if (dataReceived.StartsWith("PLAY"))
            {
                string playedCard = dataReceived.Substring(5);
            }
            else if (dataReceived.StartsWith("MSG"))
            {
                string messageReceived = dataReceived.Substring(4).Trim();

                Color playerColor = playerDatabase.GetPlayerColor(clientPlayer);

                UpdateChatBox($"{clientUsername}: ", playerColor);
                UpdateChatBox(messageReceived);
            }
        }
        private void UpdateServerLog(string message)
        {
            if (serverLog.InvokeRequired)
            {
                serverLog.BeginInvoke(new MethodInvoker(delegate { serverLog.AppendText(message); }));
            }
            else
            {
                serverLog.AppendText(message);
            }
        }
        private void UpdateChatBox(string message, Color? color = null)
        {
            if (chatBox.InvokeRequired)
            {
                chatBox.BeginInvoke(new MethodInvoker(delegate { UpdateChatBox(message, color); }));
            }
            else
            {
                if (color != null)
                {
                    chatBox.SelectionStart = chatBox.TextLength;
                    chatBox.SelectionLength = 0;
                    chatBox.SelectionColor = color.Value;
                    chatBox.AppendText(message);
                    chatBox.SelectionColor = chatBox.ForeColor;
                }
                else
                {
                    chatBox.AppendText(message + Environment.NewLine);
                }
            }
        }
        public async Task SendMessage(string message)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (IOException ioEx)
            {
                UpdateServerLog($"Error sending message to client: {ioEx.Message}{Environment.NewLine}");
            }
        }
    }
}

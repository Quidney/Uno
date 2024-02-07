using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Uno.Classes
{
    public class Server
    {
        TcpListener listener;
        PlayerDatabase playerDatabase;
        CustomTextBox serverLog;
        CardFunctionality cardFunctionality;

        public Server()
        {

        }

        public void SetReferences(PlayerDatabase playerDatabase, CustomTextBox serverLog, CardFunctionality cardFunctionality)
        {
            this.playerDatabase = playerDatabase;
            this.serverLog = serverLog;
            this.cardFunctionality = cardFunctionality;
        }

        public void CreateServer(int port)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                serverLog.AppendText($"Server starting at port: {port}{Environment.NewLine}");

                listener.BeginAcceptTcpClient(ConnectionHandling, null);
                serverLog.AppendText($"Server starting accepting TCP Clients.{Environment.NewLine}");
            }
            catch (Exception e)
            {
                //serverLog.AppendText($"Error: CreateServer: {e.Message}{Environment.NewLine}");
                throw new Exception(e.Message);
            }
        }

        private void ConnectionHandling(IAsyncResult result)
        {
            try
            {
                TcpClient client = listener.EndAcceptTcpClient(result);

                listener.BeginAcceptTcpClient(ConnectionHandling, null);

                ClientHandler clientHandler = new ClientHandler(client);
                clientHandler.SetReferences(playerDatabase, cardFunctionality, serverLog);
                clientHandler.Start();
            }
            catch (Exception e)
            {
                serverLog.AppendText($"Error: ConnectionHandling: {e.Message}{Environment.NewLine}");
            }
        }
    }

    public class ClientHandler
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread clientThread;
        private PlayerDatabase playerDatabase;
        private CardFunctionality cardFunctionality;
        private CustomTextBox serverLog;

        private string clientUsername;
        private string clientUUID;
        private Player clientPlayer;

        public ClientHandler(TcpClient client)
        {
            this.client = client;
        }

        public void SetReferences(PlayerDatabase playerDatabase, CardFunctionality cardFunctionality, CustomTextBox serverLog)
        {
            this.playerDatabase = playerDatabase;
            this.cardFunctionality = cardFunctionality;
            this.serverLog = serverLog;
        }

        public void Start()
        {
            stream = client.GetStream();
            clientThread = new Thread(HandleClient);
            clientThread.Start();
        }

        private void HandleClient()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    ProcessClientData(dataReceived);
                }
            }
            catch (Exception e)
            {
                serverLog.AppendText($"Error: HandleClient: {e.Message}{Environment.NewLine}");
            }
            finally
            {
                client.Close();
                serverLog.AppendText($"{clientUsername} Disconnected. {Environment.NewLine}");
                stream.Close();
                serverLog.AppendText($"Stream Closed.{Environment.NewLine}");

                playerDatabase.players.Remove(clientPlayer);
                playerDatabase.UuidPlayerDictionary.Remove(clientUUID);
                playerDatabase.UuidNameDictionary.Remove(clientUUID);
            }
        }

        private void ProcessClientData(string dataReceived)
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
        }

        public void SendData(string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}

using System;
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

        public Server()
        {

        }

        public void SetReferences(PlayerDatabase playerDatabase)
        {
            this.playerDatabase = playerDatabase;
        }

        public void CreateServer(int port)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                listener.BeginAcceptTcpClient(ConnectionHandling, null);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error creating server: {e.Message}" + "\nCreateServer");
            }
        }

        private void ConnectionHandling(IAsyncResult result)
        {
            try
            {
                TcpClient client = listener.EndAcceptTcpClient(result);

                listener.BeginAcceptTcpClient(ConnectionHandling, null);

                ClientHandler clientHandler = new ClientHandler(client);
                clientHandler.SetReferences(playerDatabase);
                clientHandler.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while making connection with client {e.Message}" + "\nConnectionHandling");
            }
        }
    }

    public class ClientHandler
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread clientThread;
        private PlayerDatabase playerDatabase;

        public ClientHandler(TcpClient client)
        {
            this.client = client;
        }

        public void SetReferences(PlayerDatabase playerDatabase)
        {
            this.playerDatabase = playerDatabase;
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
                MessageBox.Show($"Error handling client: {e.Message}" + "\nHandleClient");
            }
            finally
            {
                client.Close();
                stream.Close();
            }
        }

        private void ProcessClientData(string dataReceived)
        {
            if (dataReceived.StartsWith("JOIN"))
            {
                string playerName = dataReceived.Substring(5);
                playerDatabase.AddClientPlayer(playerName);
            }
        }

        public void SendData(string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Uno.Classes
{
    public class Server
    {
        TcpListener listener;
        public List<ClientHandler> connectedClients = new List<ClientHandler>();

        PlayerDatabase playerDatabase;
        CustomTextBox serverLog;
        CardFunctionality cardFunctionality;
        CustomRichTextBox chatBox;

        public Server()
        {

        }

        public void SetReferences(PlayerDatabase playerDatabase, CustomTextBox serverLog, CardFunctionality cardFunctionality, CustomRichTextBox chatBox)
        {
            this.playerDatabase = playerDatabase;
            this.serverLog = serverLog;
            this.cardFunctionality = cardFunctionality;
            this.chatBox = chatBox;
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
                serverLog.AppendText($"Error: CreateServer: {e.Message}{Environment.NewLine}");
            }
        }

        private void ConnectionHandling(IAsyncResult result)
        {
            try
            {
                TcpClient client = listener.EndAcceptTcpClient(result);

                listener.BeginAcceptTcpClient(ConnectionHandling, null);

                ClientHandler clientHandler = new ClientHandler(client);
                clientHandler.SetReferences(playerDatabase, cardFunctionality, serverLog, chatBox);
                clientHandler.ClientDisconnected += HandleClientDisconnection;
                connectedClients.Add(clientHandler);
                clientHandler.Start();
            }
            catch (Exception e)
            {
                serverLog.AppendText($"Error: ConnectionHandling: {e.Message}{Environment.NewLine}");
            }
        }
        private void HandleClientDisconnection(object sender, EventArgs e)
        {
            var disconnectedClient = (ClientHandler)sender;
            connectedClients.Remove(disconnectedClient);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uno.Classes
{
    public class ServerHost
    {
        TcpListener listener;
        List<TcpClient> clients;

        public ServerHost()
        {
            clients = new List<TcpClient>();
        }

        public void HostServer(int port)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                AcceptClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
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
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
           
        }

        private async void ClientConnection(object clientInit)
        {
            try
            {
                TcpClient client = (TcpClient)clientInit;
                NetworkStream stream = client.GetStream();

                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    ProcessMessage(message);
                    MessageBox.Show(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void ProcessMessage(string message)
        {
            try
            {
                string[] wholeMessage = message.Split(' ');
                if (wholeMessage.Length > 1)
                {
                    switch (wholeMessage[0])
                    {
                        case "MSG":
                            MessageBox.Show(message, "MESSAGES FROM THE STARS");
                            break;
                        default:
                            MessageBox.Show("UNKNOWN MESSAGE");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            
        }

    }
}
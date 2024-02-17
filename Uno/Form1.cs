using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uno.Class;
using Uno.Classes;

namespace Uno
{
    public partial class Form1 : Form
    {
        public Player currentPlayer;
        public bool joinedOrHosted = false;
        public bool isHost = false;

        public Deck deck;
        public CardFunctionality cardFunctionality;
        public PlayerDatabase playerDatabase;
        public ServerHost serverHost;
        public ServerJoin serverJoin;

        public ChatBox chatBox;

        public Form1()
        {
            InitializeComponent();

            InitMethod();
        }

        void InitMethod()
        {
            chatBox = new ChatBox();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Uno!";

            deck = new Deck();
            //deck.Shuffle();

            cardFunctionality = new CardFunctionality();

            playerDatabase = new PlayerDatabase();

            serverHost = new ServerHost();
            serverJoin = new ServerJoin();

            cardFunctionality.SetReferences(this, pnlMain);
            serverHost.SetReferences(this);
            serverJoin.SetReferences(this);
            playerDatabase.SetReferences(txtServerLog);
            chatBox.SetReferences(this);


            txtServerLog.AppendText($"Server Log: {Environment.NewLine}");
        }

        private void AddCardToGUI(Card card)
        {
            CustomLabel label = new CustomLabel()
            {
                Dock = DockStyle.Fill,
                Parent = pnlMain,
                Text = card.ToString(),
                AssignedCard = card,
                Tag = "PlayerCard"
            };

            switch (card.Type)
            {
                case Card.TypeEnum.Number:
                    label.TextAlign = ContentAlignment.TopRight;
                    label.BackColor = card.ToColor();
                    label.Font = new Font("Arial", 12);
                    break;
                case Card.TypeEnum.Action:
                    switch (card.Action)
                    {
                        case Card.ActionEnum.DrawTwo:
                            label.TextAlign = ContentAlignment.TopRight;
                            label.BackColor = card.ToColor();
                            label.Font = new Font("Arial", 12);
                            break;
                        case Card.ActionEnum.Reverse:
                        case Card.ActionEnum.Skip:
                            label.TextAlign = ContentAlignment.TopRight;
                            label.BackColor = card.ToColor();
                            label.Font = new Font("Arial", 12);
                            break;
                    }
                    break;
                case Card.TypeEnum.Wild:
                    switch (card.Wild)
                    {
                        case Card.WildEnum.DrawFour:
                            label.TextAlign = ContentAlignment.TopRight;
                            label.Font = new Font("Arial", 12);
                            break;
                        case Card.WildEnum.ChangeColor:
                            label.TextAlign = ContentAlignment.MiddleCenter;
                            label.Font = new Font("Arial", 12);
                            break;
                    }
                    label.BackColor = Color.Black;
                    label.ForeColor = Color.White;
                    break;
            }

            label.Click += (sender, e) => cardFunctionality.ThrowCardInPile(sender, e, label, currentPlayer);

            pnlMain.SetRow(label, 18);
            pnlMain.SetColumn(label, 1);

            pnlMain.SetRowSpan(label, 3);
            pnlMain.SetColumnSpan(label, 2);


            int row = 15;
            int column = 0;

            foreach (Control control in pnlMain.Controls)
            {
                if (control is CustomLabel cardLabel)
                {
                    if ((string)cardLabel.Tag == "PlayerCard")
                    {

                        if (column + 1 <= pnlMain.ColumnCount)
                        {
                            pnlMain.SetColumn(cardLabel, column + 1);
                            column++;
                        }
                        else
                        {
                            if (row + 1 <= pnlMain.RowCount)
                            {
                                pnlMain.SetRow(cardLabel, row + 1);
                                row++;
                            }
                        }
                    }
                }
            }
        }

        private void HostGame_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPortHost.Text) && !string.IsNullOrEmpty(txtUsername.Text))
            {
                if (txtUsername.Text.Length <= 24 && txtUsername.Text.Length > 4)
                {
                    string username = txtUsername.Text;

                    if (int.TryParse(txtPortHost.Text, out int port))
                    {
                        HostGame(port, username);
                    }
                    else MessageBox.Show("Invalid Port!", "Error while hosting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Username must be between 4 and 24 characters long");
                }
            }
            else
            {
                MessageBox.Show("Please fill both Host Port and Username");
            }

        }
        private void JoinGame_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Length <= 24 && txtUsername.Text.Length > 2 && !txtUsername.Text.Contains(' '))
            {
                string username = txtUsername.Text;

                if (IPAddress.TryParse(txtIPAddressJoin.Text, out IPAddress ip))
                {
                    if (int.TryParse(txtPortJoin.Text, out int port) && port > 0 && port <= 65535)
                    {
                        JoinGame(ip, port, username);
                    }
                    else
                        MessageBox.Show("Invalid Port!", "Error while joining", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Invalid IPAddress!", "Error while joining", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Username must be between 3 and 24 characters long, and cannot contain whitespaces");
            }
        }

        private async void HostGame(int port, string username)
        {
            string response;
            using (HttpClient httpClient = new HttpClient())
            {
                string api = "http://ipinfo.io/ip";
                response = await httpClient.GetStringAsync(api);
            }
            AppendLogBox($"Hosting server on {response}:{port}");

            currentPlayer = serverHost.HostServer(port, username);
            isHost = currentPlayer.IsHost;

            AppendLogBox("Server is running.");
            joinedOrHosted = true;

            chatBox.lblTitleExtern.Text = $"Chatbox - {response}:{port}";
            chatBox.Show();
        }
        private async void JoinGame(IPAddress ip, int port, string username)
        {
            AppendLogBox($"Connecting to {ip}:{port} as {username}");

            currentPlayer = await Task.Run(() => serverJoin.JoinGame(ip, port, username));

            if (currentPlayer != null)
            {
                AppendLogBox("Connected to server.");
                joinedOrHosted = true;

                chatBox.lblTitleExtern.Text = $"Chatbox - {ip}:{port}";
                chatBox.Show();
            }
            else
            {
                AppendLogBox("Failed to connect to server.");
            }
        }
       

        

        public void AppendLogBox(string message)
        {
            CustomTextBox txtBox = txtServerLog;

            txtBox.AppendText(message + Environment.NewLine);
        }

        private void customPictureBox1_Click(object sender, EventArgs e)
        {
            chatBox.Show();
        }
    }
}

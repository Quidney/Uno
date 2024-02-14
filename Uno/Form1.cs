using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Uno.Class;
using Uno.Classes;

namespace Uno
{
    public partial class Form1 : Form
    {
        Player currentPlayer;

        Deck deck;
        CardFunctionality cardFunctionality;
        PlayerDatabase playerDatabase;

        bool joinedOrHosted = false;
        ServerHost serverHost;
        ServerJoin serverJoin;

        public Form1()
        {
            InitializeComponent();

            InitMethod();
        }

        void InitMethod()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Uno!";

            deck = new Deck();
            deck.Shuffle();

            cardFunctionality = new CardFunctionality();

            playerDatabase = new PlayerDatabase();

            serverHost = new ServerHost();
            serverJoin = new ServerJoin();

            cardFunctionality.SetReferences(playerDatabase, pnlMain, this);

            serverHost.SetReferences(this);
            playerDatabase.SetReferences(txtServerLog);


            txtServerLog.AppendText($"Server Log: {Environment.NewLine}");
        }

        void StartGame()
        {
            PlaceOneCardOnThePile();
            GiveStartingCards();
        }

        void PlaceOneCardOnThePile()
        {
            Card card = deck.cardsDeckList.LastOrDefault();
            cardFunctionality.NewCardInPile(card);
            deck.cardsDeckList.Remove(card);
            cardFunctionality.currentColor = card.Color;
        }

        void GiveStartingCards()
        {
            for (int i = 0; i < 7; i++)
            {
                Card card = deck.cardsDeckList.LastOrDefault();
                currentPlayer.DrawCard(card);
                deck.cardsDeckList.Remove(card);

                AddCardToGUI(card);
            }
        }

        private void BtnStartGame_Click(object sender, EventArgs e)
        {
            StartGame();
            (sender as Button).Dispose();
        }
        private void DrawCard(object sender, EventArgs e)
        {
            if (deck.cardsDeckList.Count < 1)
            {
                foreach (Card card in cardFunctionality.cardsInPile)
                {
                    deck.cardsDeckList.Add(card);
                }
                cardFunctionality.cardsInPile = new List<Card>();

                deck.Shuffle();
            }

            Card drawnCard = deck.cardsDeckList.LastOrDefault();
            currentPlayer.DrawCard(drawnCard);
            deck.cardsDeckList.Remove(drawnCard);

            AddCardToGUI(drawnCard);

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

        private void HostGame(int port, string username)
        {
            string hostName = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
            AppendLogBox($"Hosting server on {hostName}:{port}");
            serverHost.HostServer(port);
            AppendLogBox("Server is running.");
            joinedOrHosted = true;
        }
        private async void JoinGame(IPAddress ip, int port, string username)
        {
            AppendLogBox($"Connecting to {ip}:{port} as {username}");
            await serverJoin.JoinGame(ip, port);
            AppendLogBox("Connected to server.");
            joinedOrHosted = true;
        }

        private void btnSendDataToServer_Click(object sender, EventArgs e)
        {
            if (joinedOrHosted)
            {
                if (!string.IsNullOrEmpty(txtSendDataToServer.Text))
                {
                    string message = txtSendDataToServer.Text;
                    AppendChatBox(message, Color.Blue, txtUsername.Text);
                    txtSendDataToServer.Text = string.Empty;
                    serverJoin.SendDataToServer("MSG " + txtUsername.Text + " " + message);
                }
            }
        }

        public void AppendChatBox(string message, Color? color = null, string sender = "Server")
        {
            CustomRichTextBox txtBox = txtChatBox;

            txtBox.SelectionStart = txtBox.Text.Length;
            txtBox.SelectionLength = 0;

            txtBox.SelectionColor = color ?? Color.Red;
            txtBox.AppendText($"{sender}: ");
            txtBox.SelectionColor = txtBox.ForeColor;
            txtBox.AppendText(message + Environment.NewLine);
        }

        public void AppendLogBox(string message)
        {
            CustomTextBox txtBox = txtServerLog;

            txtBox.AppendText(message + Environment.NewLine);
        }
    }
}

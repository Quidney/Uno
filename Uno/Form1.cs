using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        Server server;

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

            server = new Server();

            cardFunctionality.SetReferences(playerDatabase, pnlMain, this);
            server.SetReferences(playerDatabase, txtServerLog, cardFunctionality);
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
                        case Card.ActionEnum.Reverse: case Card.ActionEnum.Skip:
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

        private void LimitPortInput(object sender, KeyPressEventArgs e)
        {
            CustomTextBox txtBox = sender as CustomTextBox;

            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (txtBox.Text.Length >= txtBox.MaxLength)
            {
                e.Handled = true;
            }
        }

        private void LimitPortInput_MaxLimit(object sender, EventArgs e)
        {
            int port;
            CustomTextBox txtBox = sender as CustomTextBox;

            if (!int.TryParse(txtBox.Text, out port) || port < 0 || port > 65535)
            {
                if (txtBox.Text.Length > 0)
                {
                    txtBox.Text = txtBox.Text.Substring(0, txtBox.Text.Length - 1);
                    txtBox.SelectionStart = txtBox.Text.Length;
                }
            }
        }

        private void HostGame(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPortHost.Text) && !string.IsNullOrEmpty(txtUsername.Text))
            {
                if (txtUsername.Text.Length <= 24 && txtUsername.Text.Length > 4)
                {
                    try
                    {
                        int portToHost = Convert.ToInt32(txtPortHost.Text);

                        server.CreateServer(portToHost);

                        playerDatabase.AddHostPlayer(txtUsername.Text);
                    }
                    catch (Exception ex)
                    {
                        txtServerLog.AppendText($"{ex.Message}{Environment.NewLine}");
                        return;
                    }
                    
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

        private void JoinGame(object sender, EventArgs e)
        {
            try
            {
                TcpClient client = new TcpClient();
                string ip = txtIPAddressJoin.Text;
                int port = Convert.ToInt32(txtPortJoin.Text);
                client.Connect(ip, port);

                NetworkStream stream = client.GetStream();

                string joinMessage = "JOIN " + txtUsername.Text;
                byte[] joinMessageBytes = Encoding.ASCII.GetBytes(joinMessage);
                stream.Write(joinMessageBytes, 0, joinMessageBytes.Length);

                currentPlayer = playerDatabase.players.FirstOrDefault(item => item.Name == txtUsername.Text);
            }
            catch (Exception ex)
            {
                txtServerLog.AppendText($"Error: JoinGame: {ex.Message}{Environment.NewLine}");
            }
        }
    }
}

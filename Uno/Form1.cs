using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public Form1()
        {
            InitializeComponent();

            InitMethod();
        }

        void InitMethod()
        {
            deck = new Deck();
            deck.Shuffle();

            cardFunctionality = new CardFunctionality();

            playerDatabase = new PlayerDatabase();
            playerDatabase.AddPlayer("User");
            currentPlayer = playerDatabase.players.FirstOrDefault(item => item.Name == "User");

            cardFunctionality.SetReferences(playerDatabase, pnlMain);
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

        private void button1_Click(object sender, EventArgs e)
        {
            StartGame();
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

            pnlMain.SetRowSpan(label, 5);
            pnlMain.SetColumnSpan(label, 2);
        }
    }
}

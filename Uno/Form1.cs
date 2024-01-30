using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Uno.Class;
using Uno.Classes;

namespace Uno
{
    public partial class Form1 : Form
    {
        Player currentPlayer;
        Deck deck;
        CardPile cardPile;
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

            cardPile = new CardPile();

            playerDatabase = new PlayerDatabase();
            playerDatabase.AddPlayer("User");
            currentPlayer = playerDatabase.players.FirstOrDefault(item => item.Name == "User");
        }

        void StartGame() 
        {
            GiveStartingCards();
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
                foreach (Card card in cardPile.cardsInPile)
                {
                    deck.cardsDeckList.Add(card);
                }
                cardPile.cardsInPile = new List<Card>();

                deck.Shuffle();
            }

            Card drawnCard = deck.cardsDeckList.LastOrDefault();
            currentPlayer.DrawCard(drawnCard);
            deck.cardsDeckList.Remove(drawnCard);

            AddCardToGUI(drawnCard);
            
        }

        private void AddCardToGUI(Card card)
        {
            Label label = new Label()
            {
                Dock = DockStyle.Fill,
                Parent = pnlMain,
                TextAlign = ContentAlignment.TopCenter,
                Text = card.ToString(),
                BackColor = card.ToColor(),
            };

            pnlMain.SetRow(label, 18);
            pnlMain.SetColumn(label, 1);

            pnlMain.SetRowSpan(label, 5);
            pnlMain.SetColumnSpan(label, 2);
        }
    }
}

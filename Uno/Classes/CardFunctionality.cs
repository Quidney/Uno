using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;
using Uno.Class;

namespace Uno.Classes
{
    public class CardFunctionality
    {
        public TableLayoutPanel pnlCards;
        public List<Card> cardsInPile;
        public PlayerDatabase playerDatabase;
        private List<Player> players;
        private Deck deck;

        private ServerHost serverHost;

        private Form1 mainForm;

        public Card.ColorEnum currentColor;

        ColorSelectionPanel colorSelectionPanel;

        public CardFunctionality()
        {
            cardsInPile = new List<Card>();
        }

        public void SetReferences(Form1 form1, TableLayoutPanel pnlCards)
        {
            this.mainForm = form1;
            this.playerDatabase = form1.playerDatabase;
            this.pnlCards = pnlCards;
            this.players = playerDatabase.players;
            this.deck = form1.deck;
            this.serverHost = form1.serverHost;
        }
        public bool ThrowCardInPile(Card card, Player player)
        {
            bool success = false;
            switch (card.Type)
            {
                case Card.TypeEnum.Number: success = ThrownNumberCard(card, player); break;
                case Card.TypeEnum.Action: success = ThrownActionCard(card, player); break;
                case Card.TypeEnum.Wild: success = ThrownWildCard(card, player); break;
            }

            return success;
        }

        private bool ThrownNumberCard(Card card, Player player)
        {

           

            player.playerInventory.Remove(card);
            cardsInPile.Add(card);
            return true;
        }
        private bool ThrownActionCard(Card card, Player player)
        {

            player.playerInventory.Remove(card);
            cardsInPile.Add(card);
            return true;
        }
        private bool ThrownWildCard(Card card, Player player)
        {
            bool success = false;

            switch (card.Wild)
            {
                case Card.WildEnum.DrawFour:
                    Player playerToDraw = players[players.IndexOf(player) + 1];
                    DrawCardsFromDeck(playerToDraw, 4);
                    OpenColorSelector();
                    success = true;
                    break;
                case Card.WildEnum.ChangeColor:
                    OpenColorSelector();
                    success = true;
                    break;
            }

            if (success)
            {
                player.playerInventory.Remove(card);
                cardsInPile.Add(card);
            }

            return success;
        }

        public void DrawCardsFromDeck(Player player, int count)
        {
            for (int i = 0; i < count; i++) 
            {
                Card drawnCard = deck.DrawCard();
                player.AddCardToInventory(drawnCard);

                if (!player.IsHost)
                {
                    playerDatabase.PlayerClientDictionary.TryGetValue(player, out TcpClient client);
                    serverHost.SendDataToSpecificClient("DRAW " + drawnCard.ID, client);
                }
            }
        }

        public void OpenColorSelector()
        {
            colorSelectionPanel = new ColorSelectionPanel(mainForm, this);
            colorSelectionPanel.Show();
        }

        public void ChangeGameColor(Card.ColorEnum color)
        {
            currentColor = color;
        }

        public void CloseColorSelector(object sender, EventArgs e)
        {
            colorSelectionPanel.Dispose();
        }
    }
}

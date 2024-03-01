using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
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
        private ServerJoin serverJoin;

        private Form1 form1;

        public Card.ColorEnum currentColor;

        ColorSelectionPanel colorSelectionPanel;

        public CardFunctionality()
        {
            cardsInPile = new List<Card>();
        }

        public void SetReferences(Form1 form1, TableLayoutPanel pnlCards)
        {
            this.form1 = form1;
            this.playerDatabase = form1.playerDatabase;
            this.pnlCards = pnlCards;
            this.players = playerDatabase.players;
            this.deck = form1.deck;
            this.serverHost = form1.serverHost;
            this.serverJoin = form1.serverJoin;
        }
        public async Task<bool> ThrowCardInPile(Card card, Player player)
        {
            bool success = false;
            switch (card.Type)
            {
                case Card.TypeEnum.Number: success = ThrownNumberCard(card, player); break;
                case Card.TypeEnum.Action: success = ThrownActionCard(card, player); break;
                case Card.TypeEnum.Wild: success = await ThrownWildCard(card, player); break;
            }

            if (success)
                form1.lastCardPlayed = card;

            return success;
        }

        public void ThrowCardInPileForClient(Card card)
        {
            form1.lastCardPlayed = card;
        }

        private bool ThrownNumberCard(Card card, Player player)
        {
            if (card.Color == form1.lastCardPlayed.Color || card.Number == form1.lastCardPlayed.Number)
            {
                player.Inventory.Remove(card);
                cardsInPile.Add(card);
                return true;
            }
            else
            {
                return false;
            }

        }
        private bool ThrownActionCard(Card card, Player player)
        {
            if (card.Color == form1.lastCardPlayed.Color || card.Action == form1.lastCardPlayed.Action)
            {
                player.Inventory.Remove(card);
                cardsInPile.Add(card);
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task<bool> ThrownWildCard(Card card, Player player)
        {
            bool success = false;

            switch (card.Wild)
            {
                case Card.WildEnum.DrawFour:
                    if (player.IsHost)
                    {
                        Draw4(player);
                    }
                    else
                    {
                        await serverJoin.SendDataToServer($"DRAW4 {player.Name}");
                    }
                    
                    if (form1.InvokeRequired)
                        form1.Invoke(new Action(OpenColorSelector));
                    else
                        OpenColorSelector();
                    success = true;
                    break;
                case Card.WildEnum.ChangeColor:
                    if (form1.InvokeRequired)
                        form1.Invoke(new Action(OpenColorSelector));
                    else
                        OpenColorSelector();
                    OpenColorSelector();
                    success = true;
                    break;
            }

            if (success)
            {
                player.Inventory.Remove(card);
                cardsInPile.Add(card);
            }

            return success;
        }

        public void Draw4(Player player)
        {
            Player playerToDraw = players[players.IndexOf(player) + 1];
            DrawCardsFromDeck(playerToDraw, 4);
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
            colorSelectionPanel = new ColorSelectionPanel(form1);
            colorSelectionPanel.Show();
            colorSelectionPanel.BringToFront();
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

using System;
using System.Collections.Generic;
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
        private ServerJoin serverJoin;

        private frmUno form1;

        public Card.ColorEnum currentColor;

        ColorSelectionPanel colorSelectionPanel;

        public bool canPlay = false;

        public CustomLabel currentColorLabel;

        public CardFunctionality()
        {
            cardsInPile = new List<Card>();
        }

        public void SetReferences(frmUno form1, TableLayoutPanel pnlCards)
        {
            this.form1 = form1;
            this.playerDatabase = form1.playerDatabase;
            this.pnlCards = pnlCards;
            this.players = playerDatabase.players;
            this.deck = form1.deck;
            this.serverHost = form1.serverHost;
            this.serverJoin = form1.serverJoin;

            currentColorLabel = new CustomLabel()
            {
                Parent = pnlCards,
                Text = string.Empty,
            };
        }
        public bool ThrowCardInPile(Card card, Player player)
        {
            if (form1.currentPlayer.IsHost && player != form1.currentPlayer) canPlay = true;

            if (canPlay)
            {
                bool success = false;
                switch (card.Type)
                {
                    case Card.TypeEnum.Number: success = ThrownNumberCard(card, player); break;
                    case Card.TypeEnum.Action: success = ThrownActionCard(card, player); break;
                    case Card.TypeEnum.Wild: success = ThrownWildCard(card, player); break;
                }

                if (success)
                {
                    form1.lastCardPlayed = card;
                    if (card.Color != Card.ColorEnum.Black)
                        currentColor = card.Color;

                    canPlay = false;

                    string nameNewPlayer = string.Empty;

                    if (form1.isHost)
                    {
                        Player newTurnPlayer = PlayerTurn(player, card.Action == Card.ActionEnum.Skip, card.Action == Card.ActionEnum.Reverse);
                        nameNewPlayer = newTurnPlayer.Name;
                    }

                    if (form1.currentPlayer == player)
                    {
                        if (form1.InvokeRequired)
                            form1.Invoke(new Action(() => { form1.Text = "Uno!"; }));
                        else
                            form1.Text = $"Uno! {nameNewPlayer}'s Turn!";
                    }

                    if (form1.currentPlayer.IsHost)
                    {
                        if (player.Inventory.Count == 0)
                        {
                            if (!player.SaidUno)
                            {
                                for (int i = 0; i < 4; i++)
                                    DrawCardsFromDeck(player, 1);
                            }
                            else
                            {
                                serverHost.BroadcastData($"WIN {player.Name}");
                                if (form1.InvokeRequired)
                                    form1.Invoke(new Action(() => { form1.GameWon(player.Name); }));
                                else
                                    form1.GameWon(player.Name);
                            }
                        }
                    }

                    currentColorLabel.Text = currentColor.ToString();

                    player.turn = false;
                }

                return success;
            }
            else
            {
                return false;
            }
        }

        public Player PlayerTurn(Player player, bool skip, bool reverse)
        {
            if (reverse)
                playerDatabase.players.Reverse();

            int playerIndex = playerDatabase.players.IndexOf(player);
            int next;

            if (!skip)
                next = (playerIndex + 1) % playerDatabase.players.Count;
            else
                next = (playerIndex + 2) % playerDatabase.players.Count;

            Player turnPlayer = playerDatabase.players[next];

            if (form1.currentPlayer != turnPlayer)
            {
                playerDatabase.PlayerClientDictionary.TryGetValue(turnPlayer, out TcpClient client);
                serverHost.SendDataToSpecificClient("TURN", client);
                serverHost.SendDataToAllExcept($"TURNOTHER {turnPlayer.Name}", client);
                form1.Text = $"Uno! {turnPlayer.Name}'s Turn!";
                turnPlayer.turn = true;
            }
            else
            {
                form1.Text = "Uno! Your Turn!";
                serverHost.BroadcastData($"TURNOTHER {form1.currentPlayer.Name}");
                canPlay = true;
                form1.currentPlayer.turn = true;
            }

            return turnPlayer;
        }

        public void ThrowCardInPileForClient(Card card)
        {
            form1.lastCardPlayed = card;
            if (card.Color != Card.ColorEnum.Black)
                currentColor = card.Color;
        }

        private bool ThrownNumberCard(Card card, Player player)
        {
            if (card.Color == currentColor || card.Number == form1.lastCardPlayed.Number)
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
            if (card.Color == currentColor || card.Action == form1.lastCardPlayed.Action)
            {
                if (card.Action == Card.ActionEnum.DrawTwo)
                {
                    if (form1.currentPlayer.IsHost)
                    {
                        int indexOfPlayer = playerDatabase.players.IndexOf(player);
                        if (indexOfPlayer + 1 == playerDatabase.players.Count)
                        {
                            DrawCardsFromDeck(playerDatabase.players[0], 2);
                        }
                        else
                        {
                            DrawCardsFromDeck(playerDatabase.players[indexOfPlayer + 1], 2);
                        }
                    }
                }

                player.Inventory.Remove(card);
                cardsInPile.Add(card);
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool ThrownWildCard(Card card, Player player)
        {
            bool success = false;

            switch (card.Wild)
            {
                case Card.WildEnum.DrawFour:
                    if (form1.currentPlayer.IsHost)
                    {
                        int indexOfPlayer = playerDatabase.players.IndexOf(player);
                        if (indexOfPlayer + 1 == playerDatabase.players.Count)
                        {
                            DrawCardsFromDeck(playerDatabase.players[0], 4);
                        }
                        else
                        {
                            DrawCardsFromDeck(playerDatabase.players[indexOfPlayer + 1], 4);
                        }
                    }
                    success = true;
                    break;
                case Card.WildEnum.ChangeColor:
                    success = true;
                    break;
            }


            if (form1.currentPlayer == player)
            {
                if (form1.InvokeRequired)
                    form1.Invoke(new Action(OpenColorSelector));
                else
                    OpenColorSelector();
            }

            if (success)
            {
                player.Inventory.Remove(card);
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

                player.SaidUno = false;

                if (form1.currentPlayer.IsHost && !player.IsHost)
                {
                    playerDatabase.PlayerClientDictionary.TryGetValue(player, out TcpClient client);
                    serverHost.SendDataToSpecificClient("DRAW " + drawnCard.ID, client);
                }
            }
        }

        public void OpenColorSelector()
        {
            colorSelectionPanel = new ColorSelectionPanel(form1);
            DialogResult colorDialog = colorSelectionPanel.ShowDialog();

            colorSelectionPanel.Dispose();

            if (colorDialog != DialogResult.OK)
                OpenColorSelector();
        }

        public async void ChangeGameColor(Card.ColorEnum color)
        {
            currentColor = color;
            if (form1.currentPlayer.IsHost)
            {
                await serverHost.BroadcastData("CHANGECOLOR " + color.ToString());
            }
            else
            {
                await serverJoin.SendDataToServer("CHANGECOLOR " + color.ToString());
            }
        }
    }
}

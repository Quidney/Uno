using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Uno.Classes
{
    public class CardFunctionality
    {
        public TableLayoutPanel pnlCards;
        public CustomLabel label;
        public List<Card> cardsInPile;
        public PlayerDatabase playerDatabase;

        private frmUno mainForm;

        public Card.ColorEnum currentColor;

        ColorSelectionPanel colorSelectionPanel;

        public CardFunctionality()
        {
            cardsInPile = new List<Card>();

        }

        public void SetReferences(PlayerDatabase playerDatabase, TableLayoutPanel pnlCards, frmUno form1)
        {
            this.playerDatabase = playerDatabase;
            this.pnlCards = pnlCards;

            this.mainForm = form1;

            label = new CustomLabel()
            {
                Dock = DockStyle.Fill,
                Parent = pnlCards,
                Text = string.Empty
            };

            pnlCards.SetRow(label, 1);
            pnlCards.SetColumn(label, 1);

            pnlCards.SetRowSpan(label, 3);
            pnlCards.SetColumnSpan(label, 2);
        }
        public void ThrowCardInPile(object sender, EventArgs e, CustomLabel label, Player currentPlayer)
        {
            Card lastCardInPile = cardsInPile.LastOrDefault();
            Card thrownCard = label.AssignedCard;

            switch (thrownCard.Type)
            {
                case Card.TypeEnum.Number:
                    if (thrownCard.Number == lastCardInPile.Number || thrownCard.Color == currentColor)
                    {
                        // currentPlayer.playerInventory.Remove(thrownCard);
                        NewCardInPile(thrownCard);
                        label.Dispose();
                        currentColor = thrownCard.Color;
                    }
                    break;
                case Card.TypeEnum.Action:
                    if (thrownCard.Action == lastCardInPile.Action || thrownCard.Color == currentColor)
                    {
                        // currentPlayer.playerInventory.Remove(thrownCard);
                        NewCardInPile(thrownCard);
                        label.Dispose();
                        currentColor = thrownCard.Color;
                    }
                    break;
                case Card.TypeEnum.Wild:
                    if (thrownCard.Wild == Card.WildEnum.DrawFour)
                    {

                    }
                    else if (thrownCard.Wild == Card.WildEnum.ChangeColor)
                    {

                    }

                    colorSelectionPanel = new ColorSelectionPanel(mainForm, this);
                    colorSelectionPanel.Show();
                    colorSelectionPanel.BringToFront();

                    // currentPlayer.playerInventory.Remove(thrownCard);
                    NewCardInPile(thrownCard);
                    label.Dispose();
                    break;
            }
        }

        public void NewCardInPile(Card card)
        {
            cardsInPile.Add(card);

            label.Text = card.ToString();
            label.AssignedCard = card;

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

        }

        public void ChangeGameColor(object sender, EventArgs e, Card.ColorEnum color)
        {
            currentColor = color;
        }

        public void CloseColorSelector(object sender, EventArgs e)
        {
            colorSelectionPanel.Dispose();
        }
    }
}

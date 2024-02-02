using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Windows.Forms;

namespace Uno.Classes
{
    public class CardFunctionality
    {
        public TableLayoutPanel pnlCards;
        public CustomLabel label;
        public List<Card> cardsInPile;
        public PlayerDatabase playerDatabase;

        public CardFunctionality()
        {
            cardsInPile = new List<Card>();

        }

        public void SetReferences(PlayerDatabase playerDatabase, TableLayoutPanel pnlCards)
        {
            this.playerDatabase = playerDatabase;
            this.pnlCards = pnlCards;

            label = new CustomLabel()
            {
                Dock = DockStyle.Fill,
                Parent = pnlCards,
                Text = string.Empty
            };

            pnlCards.SetRow(label, 1);
            pnlCards.SetColumn(label, 1);
        }
        public void ThrowCardInPile(object sender, EventArgs e, CustomLabel label, Player currentPlayer)
        {
            Card lastCardInPile = cardsInPile.LastOrDefault();

            switch (lastCardInPile.Type)
            {
                case Card.TypeEnum.Number:
                    if (label.AssignedCard.Number == lastCardInPile.Number || label.AssignedCard.Color == lastCardInPile.Color)
                    {
                        currentPlayer.playerInventory.Remove(label.AssignedCard);
                        label.Dispose();
                        NewCardInPile(label.AssignedCard);
                    }
                    break;
                case Card.TypeEnum.Action:
                    if (label.AssignedCard.Color == lastCardInPile.Color || label.AssignedCard.Action == lastCardInPile.Action)
                    {
                        currentPlayer.playerInventory.Remove(label.AssignedCard);
                        label.Dispose();
                        NewCardInPile(label.AssignedCard);
                    }
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
    }
}
